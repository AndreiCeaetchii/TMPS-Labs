using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Services;

public class BookingRegistry
{
    private static readonly Lazy<BookingRegistry> Lazy = new(() => new BookingRegistry());
    public static BookingRegistry Instance => Lazy.Value;

    private readonly List<Booking> _bookings = new();

    private BookingRegistry() { }

    public IReadOnlyList<Booking> AllBookings => _bookings.AsReadOnly();

    public void Add(Booking booking)
    {
        _bookings.Add(booking);
    }

    public bool HasConflict(Room room, DateTime start, DateTime end)
    {
        return _bookings.Any(b =>
            b.Room.Code == room.Code &&
            start < b.End &&
            end > b.Start);
    }

    public void PrintAll()
    {
        Console.WriteLine("\n--- Current Bookings ---");
        foreach (var b in _bookings)
            Console.WriteLine(b);
    } 
}