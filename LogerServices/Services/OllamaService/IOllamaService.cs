namespace LogerServices.Services
{
    public interface IOllamaService
    {
        Task<string> AskAiAsync(string logMessage);
    }
}