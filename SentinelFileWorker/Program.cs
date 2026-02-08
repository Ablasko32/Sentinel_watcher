using SentinelFileWorker;
using SentinelFileWorker.Startup;

var builder = Host.CreateApplicationBuilder(args);
//Options
builder.ConfigureOptions();

//Ollama and Telegram clients, NotificationService
builder.ConfigureSingeltons();

//DB
builder.ConfigureDB();

//Repositories
builder.ConfigureRepositores();

//parsers
builder.ConfigureParsers();

//Worker

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();