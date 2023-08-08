using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using WeatherForecast.Client.Core.Application.Interfaces;
using WeatherForecast.Client.Core.Domain.Models.Notifications;
using Telegram.Bot.Types;

namespace WeatherForecast.Client.TelegramBot.Telegram
{
    public class TelegramClient
    {
        public TelegramClient(IUserManager userManager, MessageSender messageSender)
        {
            _userManager = userManager;
            _messageSender = messageSender;
        }

        private readonly IUserManager _userManager;
        private readonly MessageSender _messageSender;

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.MyChatMember is { } chatmember)
            {
                if (chatmember.NewChatMember.Status == ChatMemberStatus.Kicked)
                {
                    await _userManager.Remove(chatmember.Chat.Id);
                    return;
                }
                await _userManager.Add(chatmember.Chat.Id);
            }
            if (update.Message is not { } message)
                return;
            if (message.Text?.ToLowerInvariant() == "/start")
                return;
            var response = await _userManager.HandleMessage(message.Chat.Id, message);
            if (response.HasError)
            {
                await _messageSender.SendErrorMessageAsync(message.Chat.Id, response.Error, cancellationToken);
                return;
            }
            await _messageSender.SendMessageAsync(message.Chat.Id, response.Result, cancellationToken);
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        public async Task<User> StartAsync(ITelegramBotClient botClient, CancellationToken token)
        {
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: token
            );
            return await botClient.GetMeAsync();
        }

        public async Task HandleNotifications(List<Notification> notifications, CancellationToken cancellationToken)
        {
            foreach (var notification in notifications)
            {
                if (!notification.Response.HasError)
                {
                    await _messageSender.SendMessageAsync(notification.UserId, notification.Response.Result, cancellationToken);
                }
            }
        }

        public async Task UpdateConnection()
        {
            await _messageSender.GetBotInfo();
        }
    }
}
