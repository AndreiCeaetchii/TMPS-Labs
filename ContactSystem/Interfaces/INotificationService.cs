namespace MovieBookingSystem.Interfaces;

public interface INotificationService
{
    Task SendNotification(string recipient, string message);
}