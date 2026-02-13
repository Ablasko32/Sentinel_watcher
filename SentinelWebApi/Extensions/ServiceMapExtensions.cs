using SentinelWebApi.Services;

namespace SentinelWebApi.Extensions
{
    public static class ServiceMapExtensions
    {
        public static WebApplicationBuilder ConfigureServiceLayer(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILogErrorService, LogErrorService>();
            return builder;
        }
    }
}