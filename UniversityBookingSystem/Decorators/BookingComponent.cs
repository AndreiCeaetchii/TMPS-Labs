using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public class BookingComponent : IBookingComponent
{
    private readonly Booking _booking;
    private const decimal BaseRoomCost = 50.0m;

    public BookingComponent(Booking booking)
    {
        _booking = booking;
    }

    public Booking Booking => _booking;

    public virtual string GetDescription()
    {
        return $"Room {_booking.Room.Code} ({_booking.Room.Type})";
    }

    public virtual decimal GetCost()
    {
        var hours = (_booking.End - _booking.Start).TotalHours;
        return BaseRoomCost * (decimal)hours;
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"Booking: {_booking}");
        Console.WriteLine($"Description: {GetDescription()}");
        Console.WriteLine($"Base Cost: ${GetCost():F2}");
    }
}
