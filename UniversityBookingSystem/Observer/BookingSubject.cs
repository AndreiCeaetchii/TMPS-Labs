using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Observer;

public class BookingSubject
{
    private readonly List<IBookingObserver> _observers = new();

    public void Attach(IBookingObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"[SUBJECT] Observer attached: {observer.GetType().Name}");
        }
    }

    public void Detach(IBookingObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
            Console.WriteLine($"[SUBJECT] Observer detached: {observer.GetType().Name}");
        }
    }

    public void NotifyBookingCreated(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Created",
            string.IsNullOrEmpty(message) ? $"Booking created for room {booking.Room.Code}" : message);

        foreach (var observer in _observers)
        {
            observer.OnBookingCreated(args);
        }
    }

    public void NotifyBookingApproved(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Approved",
            string.IsNullOrEmpty(message) ? $"Booking approved for room {booking.Room.Code}" : message);

        foreach (var observer in _observers)
        {
            observer.OnBookingApproved(args);
        }
    }

    public void NotifyBookingCancelled(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Cancelled",
            string.IsNullOrEmpty(message) ? $"Booking cancelled for room {booking.Room.Code}" : message);

        foreach (var observer in _observers)
        {
            observer.OnBookingCancelled(args);
        }
    }

    public void NotifyBookingModified(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Modified",
            string.IsNullOrEmpty(message) ? $"Booking modified for room {booking.Room.Code}" : message);

        foreach (var observer in _observers)
        {
            observer.OnBookingModified(args);
        }
    }

    public void NotifyBookingCompleted(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Completed",
            string.IsNullOrEmpty(message) ? $"Booking completed for room {booking.Room.Code}" : message);

        foreach (var observer in _observers)
        {
            observer.OnBookingCompleted(args);
        }
    }

    public int ObserverCount => _observers.Count;
}
