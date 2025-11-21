using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public abstract class BaseBookingDecorator : IBookingComponent
{
    protected readonly IBookingComponent WrappedBooking;

    protected BaseBookingDecorator(IBookingComponent booking)
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
