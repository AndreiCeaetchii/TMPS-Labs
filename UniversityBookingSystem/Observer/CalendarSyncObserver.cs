namespace UniversityBookingSystem.Observer;

public class CalendarSyncObserver : IBookingObserver
{
    private readonly Dictionary<string, string> _calendarEntries = new();

    public void OnBookingCreated(BookingEventArgs args)
    {
        var calendarId = AddToCalendar(args.Booking);
        Console.WriteLine($"  [CALENDAR] Added to calendar: {calendarId}");
    }

    public void OnBookingApproved(BookingEventArgs args)
    {
        UpdateCalendarEntry(args.Booking, "Confirmed");
        Console.WriteLine($"  [CALENDAR] Updated calendar entry status to Confirmed");
    }

    public void OnBookingCancelled(BookingEventArgs args)
    {
        RemoveFromCalendar(args.Booking);
        Console.WriteLine($"  [CALENDAR] Removed from calendar");
    }

    public void OnBookingModified(BookingEventArgs args)
    {
        UpdateCalendarEntry(args.Booking, "Modified");
        Console.WriteLine($"  [CALENDAR] Updated calendar entry");
    }

    public void OnBookingCompleted(BookingEventArgs args)
    {
        UpdateCalendarEntry(args.Booking, "Completed");
        Console.WriteLine($"  [CALENDAR] Marked as completed in calendar");
    }

    private string AddToCalendar(Entities.Booking booking)
    {
        var calendarId = $"CAL-{booking.Room.Code}-{DateTime.Now.Ticks}";
        _calendarEntries[calendarId] = $"{booking.Room.Code} ({booking.Start} - {booking.End})";
        return calendarId;
    }

    private void UpdateCalendarEntry(Entities.Booking booking, string status)
    {
        var key = _calendarEntries.Keys.FirstOrDefault(k => k.Contains(booking.Room.Code));
        if (key != null)
        {
            _calendarEntries[key] = $"{booking.Room.Code} - {status}";
        }
    }

    private void RemoveFromCalendar(Entities.Booking booking)
    {
        var key = _calendarEntries.Keys.FirstOrDefault(k => k.Contains(booking.Room.Code));
        if (key != null)
        {
            _calendarEntries.Remove(key);
        }
    }
}
