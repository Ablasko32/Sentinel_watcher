using LogerServices.Configuration;
using LogerServices.Services;
using LogWorker;
using LogWorker.Configuration;
using Telegram.Bot;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.Configure<OllamaOptions>(builder.Configuration.GetSection("Ollama"));
builder.Services.Configure<LogerWorkerOptions>(builder.Configuration.GetSection("LoggerWorker"));
builder.Services.Configure<TelegramOptions>(builder.Configuration.GetSection("TelegramOptions"));

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

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();