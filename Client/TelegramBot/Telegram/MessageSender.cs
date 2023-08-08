using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Client.Core.Domain.Models.Enums;
using WeatherForecast.Client.Core.Domain.Models;

using WeatherForecast.Client.TelegramBot.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;

namespace WeatherForecast.Client.TelegramBot.Telegram
{
    public class MessageSender
    {
        private readonly ITelegramBotClient _botClient;
        private readonly IConfiguration _configuration;
        private readonly IAttachmentConverter _attachmentConverter;

        public MessageSender(ITelegramBotClient botClient, IConfiguration configuration, IAttachmentConverter attachmentConverter)
        {
            _botClient = botClient;
            _configuration = configuration;
            _attachmentConverter = attachmentConverter;
        }

        public async Task<User> GetBotInfo() => await _botClient.GetMeAsync();

        private IReplyMarkup GetReplyKeyboard(MessageType messageType)
            => messageType == MessageType.LocationRequest ?
                    new ReplyKeyboardMarkup(new[]
                    {
                        new KeyboardButton("Поделиться местоположением")
                        {
                            RequestLocation = true
                        }
                    })
                    {
                        ResizeKeyboard = true
                    } : new ReplyKeyboardRemove();

        public async Task SendMessageAsync(long chatId, OutputMessage message, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: $"{_configuration.GetRequiredSection("OutputMessages")[message.Response.ToString()]!}\n",
                    replyMarkup: GetReplyKeyboard(message.Response),
                    cancellationToken: cancellationToken);
            if (!string.IsNullOrEmpty(message.AdditionalInformation))
                await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: message.AdditionalInformation,
                    cancellationToken: cancellationToken);
            if (message.Attachments != null)
                foreach (var attachment in message.Attachments)
                {
                    var path = await _attachmentConverter.Convert(attachment);
                    using (FileStream fstream = new FileStream(path, FileMode.Open))
                    {
                        await _botClient.SendPhotoAsync(
                            chatId: chatId,
                            new InputOnlineFile(fstream, path),
                            cancellationToken: cancellationToken);
                    }
                    System.IO.File.Delete(path);
                }
        }

        public async Task SendErrorMessageAsync(long chatId, ErrorMessageType error, CancellationToken cancellationToken)
        {
            await _botClient.SendTextMessageAsync(
                    chatId: chatId,
                    text: _configuration.GetRequiredSection("ErrorMessages")[error.ToString()]!,
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
        }
    }
}
