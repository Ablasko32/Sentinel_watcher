using Microsoft.EntityFrameworkCore;
using SentinelCore.Configuration;
using SentinelCore.DAL;
using SentinelCore.DAL.Repositories;
using SentinelCore.Services;
using SentinelCore.Services.ParserService;
using SentinelFileWorker.Configuration;
using Telegram.Bot;

namespace SentinelFileWorker.Startup
{
    public static class StartupExtensions
    {
        public static IHostApplicationBuilder ConfigureDB(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<SentinelContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                if (String.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException(nameof(connectionString));
                }
                options.UseSqlite(connectionString);
            });
            return builder;
        }

        public static IHostApplicationBuilder ConfigureSingeltons(this IHostApplicationBuilder builder)
        {
            builder.Services.AddSingleton<ITelegramBotClient>(opt =>
            {
                var telegramToken = builder.Configuration["TelegramOptions:TelegramToken"];
                if (String.IsNullOrEmpty(telegramToken))
                {
                    throw new ArgumentNullException(nameof(telegramToken));
                }
                return new TelegramBotClient(telegramToken);
            });

            builder.Services.AddSingleton<IOllamaService, OllamaService>();
            builder.Services.AddSingleton<INotificationService, NotificationService>();
            return builder;
        }

        public static IHostApplicationBuilder ConfigureOptions(this IHostApplicationBuilder builder)
        {
            builder.Services.Configure<OllamaOptions>(builder.Configuration.GetSection("Ollama"));
            builder.Services.Configure<LogerWorkerOptions>(builder.Configuration.GetSection("LoggerWorker"));
            builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection("TelegramOptions"));
            return builder;
        }

        public static IHostApplicationBuilder ConfigureRepositores(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILogErrorRepository, LogErrorRepository>();
            return builder;
        }

        public static IHostApplicationBuilder ConfigureParsers(this IHostApplicationBuilder builder)
        {
            builder.Services.AddScoped<INetParserService, NetParserService>();
            return builder;
        }
    }
}