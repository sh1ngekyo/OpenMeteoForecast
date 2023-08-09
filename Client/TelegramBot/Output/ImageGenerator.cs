using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Client.TelegramBot.Output
{
    public class ImageGenerator
    {
        private static string _toolFileName;
        private static string _directory;
        private static string _toolFilePath;

        static ImageGenerator()
        {
            _toolFileName = "wkhtmltoimage";
            _directory = AppContext.BaseDirectory;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _toolFilePath = Path.Combine(_directory, _toolFileName + ".exe");
                if (File.Exists(_toolFilePath))
                {
                    return;
                }

                Assembly assembly = typeof(HtmlConverter).GetTypeInfo().Assembly;
                string @namespace = typeof(HtmlConverter)!.Namespace;
                using (Stream stream = assembly.GetManifestResourceStream(@namespace + "." + _toolFileName + ".exe"))
                {
                    using FileStream destination = File.OpenWrite(_toolFilePath);
                    stream.CopyTo(destination);
                }

                return;
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process? process = Process.Start(new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WorkingDirectory = "/bin/",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = "/bin/bash",
                    Arguments = "which wkhtmltoimage"
                });
                string text = process!.StandardOutput.ReadToEnd();
                process!.WaitForExit();
                if (!string.IsNullOrEmpty(text) && text.Contains("wkhtmltoimage"))
                {
                    _toolFilePath = "wkhtmltoimage";
                    return;
                }

                throw new Exception("wkhtmltoimage does not appear to be installed on this linux system according to which command; go to https://wkhtmltopdf.org/downloads.html");
            }

            throw new Exception("OSX Platform not implemented yet");
        }

        public enum ImageFormat
        {
            Jpg,
            Png
        }

        public byte[] FromHtmlString(string html, int width = 800, ImageFormat format = ImageFormat.Jpg, int quality = 100)
        {
            string text = Path.Combine(_directory, $"{Guid.NewGuid()}.html");
            File.WriteAllText(text, html);
            byte[] result = FromUrl(text, width, format, quality);
            File.Delete(text);
            return result;
        }

        public byte[] FromUrl(string url, int width = 800, ImageFormat format = ImageFormat.Jpg, int quality = 100)
        {
            string text = format.ToString().ToLower();
            string text2 = Path.Combine(_directory, Guid.NewGuid().ToString() + "." + text);
            Process? process = Process.Start(new ProcessStartInfo(arguments: (!IsLocalPath(url)) ? $"--enable-local-file-access --quality {quality} --width {width} -f {text} {url} \"{text2}\"" : $"--enable-local-file-access  --quality {quality} --width {width} -f {text} \"{url}\" \"{text2}\"", fileName: _toolFilePath)
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = _directory,
                RedirectStandardError = true
            });
            process!.ErrorDataReceived += new DataReceivedEventHandler(Process_ErrorDataReceived);
            process!.WaitForExit();
            if (File.Exists(text2))
            {
                byte[] result = File.ReadAllBytes(text2);
                File.Delete(text2);
                return result;
            }

            throw new Exception("Something went wrong. Please check input parameters");
        }

        private static bool IsLocalPath(string path)
            => path.StartsWith("http") ? false : new Uri(path).IsFile;

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
            => throw new Exception(e.Data);
    }
}
