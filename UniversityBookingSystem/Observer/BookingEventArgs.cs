using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Observer;

public class BookingEventArgs
{
    public Booking Booking { get; set; }
    public string EventType { get; set; }
    public DateTime EventTime { get; set; }
    public string Message { get; set; }
    public string? PreviousState { get; set; }
    public string? NewState { get; set; }

    public BookingEventArgs(Booking booking, string eventType, string message)
    {
        Booking = booking;
        EventType = eventType;
        Message = message;
        EventTime = DateTime.Now;
    }
}
