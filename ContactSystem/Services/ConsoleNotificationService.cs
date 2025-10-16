using MovieBookingSystem.Interfaces;

namespace MovieBookingSystem.Services;

public class ConsoleNotificationService : INotificationService
{
    public Task SendNotification(string recipient, string message)
    {
        Console.WriteLine($"[Notification → {recipient}] {message}");
        return Task.CompletedTask;
    }
}