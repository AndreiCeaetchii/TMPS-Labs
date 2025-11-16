using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Decorators;

/// <summary>
/// Concrete component that wraps the base Booking entity.
/// This is the base object that decorators will wrap.
/// </summary>
public class BaseBookingComponent : IBookingComponent
{
    private readonly Booking _booking;
    private const decimal BaseRoomCost = 50.0m; // Base cost per hour

    public BaseBookingComponent(Booking booking)
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
