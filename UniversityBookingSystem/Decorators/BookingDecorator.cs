using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public abstract class BookingDecorator : IBookingComponent
{
    protected readonly IBookingComponent WrappedBooking;

    protected BookingDecorator(IBookingComponent booking)
    {
        WrappedBooking = booking;
    }

    public virtual string GetDescription()
    {
        return WrappedBooking.GetDescription();
    }

    public virtual decimal GetCost()
    {
        return WrappedBooking.GetCost();
    }

    public virtual void DisplayDetails()
    {
        WrappedBooking.DisplayDetails();
    }
}
