namespace LogerServices.Services
{
    public interface INotificationService
    {
        Task SendNotificationAsync(string message);
    }
}