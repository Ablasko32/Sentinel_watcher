namespace SentinelCore.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string message);
    }
}