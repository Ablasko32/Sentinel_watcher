using LogerServices.Configuration;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace LogerServices.Services
{
    public class NotificationService: INotificationService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly TelegramOptions _options;

        public NotificationService(ITelegramBotClient botClient, IOptions<TelegramOptions> options)
        {
            _botClient = botClient;
            _options = options.Value;
        }

        public async Task SendNotificationAsync(string message)
        {
            var header = "🚨 ERROR IN LOGS DETECTED 🚨\n";
            header += "🤖 AI ANALYSIS REPORT\n";
            header += "--------------------------------\n\n";

            var fullMessage = header + message;
            await _botClient.SendMessage(
                chatId: _options.TelegramChatId,
                text: fullMessage,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.None);
        }
    }
}