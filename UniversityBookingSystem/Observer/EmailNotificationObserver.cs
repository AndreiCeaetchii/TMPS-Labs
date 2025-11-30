namespace UniversityBookingSystem.Observer;

public class EmailNotificationObserver : IBookingObserver
{
    private readonly string _observerName;

    public EmailNotificationObserver(string observerName = "Email Notifier")
    {
        _observerName = observerName;
    }

    public void OnBookingCreated(BookingEventArgs args)
    {
        SendEmail($"New Booking Created: {args.Booking}");
        LogNotification(args, "CREATED");
    }

    public void OnBookingApproved(BookingEventArgs args)
    {
        SendEmail($"Booking Approved: {args.Booking}");
        LogNotification(args, "APPROVED");
    }

    public void OnBookingCancelled(BookingEventArgs args)
    {
        SendEmail($"Booking Cancelled: {args.Booking}");
        LogNotification(args, "CANCELLED");
    }

    public void OnBookingModified(BookingEventArgs args)
    {
        SendEmail($"Booking Modified: {args.Booking}");
        LogNotification(args, "MODIFIED");
    }

    public void OnBookingCompleted(BookingEventArgs args)
    {
        SendEmail($"Booking Completed: {args.Booking}");
        LogNotification(args, "COMPLETED");
    }

    private void SendEmail(string content)
    {
        Console.WriteLine($"  [EMAIL] Sending notification to {content}");
    }

    private void LogNotification(BookingEventArgs args, string action)
    {
        Console.WriteLine($"  [{_observerName}] {action} - {args.Message} at {args.EventTime:HH:mm:ss}");
    }
}
