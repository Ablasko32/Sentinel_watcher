using SentinelCore.DAL.Repositories;

namespace SentinelWebApi.Extensions
{
    public static class RepositoryMapExtensions
    {
        public static WebApplicationBuilder ConfigureRepositoryLayer(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ILogErrorRepository, LogErrorRepository>();
            return builder;
        }
    }
}