namespace UniversityBookingSystem.Observer;

public class LoggingObserver : IBookingObserver
{
    private readonly List<string> _logEntries = new();

    public void OnBookingCreated(BookingEventArgs args)
    {
        LogEvent(args, "BOOKING_CREATED");
    }

    public void OnBookingApproved(BookingEventArgs args)
    {
        LogEvent(args, "BOOKING_APPROVED");
    }

    public void OnBookingCancelled(BookingEventArgs args)
    {
        LogEvent(args, "BOOKING_CANCELLED");
    }

    public void OnBookingModified(BookingEventArgs args)
    {
        LogEvent(args, "BOOKING_MODIFIED");
    }

    public void OnBookingCompleted(BookingEventArgs args)
    {
        LogEvent(args, "BOOKING_COMPLETED");
    }

    private void LogEvent(BookingEventArgs args, string eventType)
    {
        var logEntry = $"[{args.EventTime:yyyy-MM-dd HH:mm:ss}] {eventType}: Room {args.Booking.Room.Code} - {args.Message}";
        _logEntries.Add(logEntry);
        Console.WriteLine($"  [LOG] {logEntry}");
    }

    public void PrintLogs()
    {
        Console.WriteLine("\n=== Booking Event Logs ===");
        foreach (var entry in _logEntries)
        {
            Console.WriteLine(entry);
        }
    }

    public IReadOnlyList<string> GetLogs() => _logEntries.AsReadOnly();
}
