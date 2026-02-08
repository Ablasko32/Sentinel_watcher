namespace SentinelCore.Services
{
    public interface IOllamaService
    {
        Task<string> AskAiAsync(string logMessage);
    }
}