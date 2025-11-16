namespace UniversityBookingSystem.Decorators;

/// <summary>
/// Abstract base decorator class.
/// All concrete decorators inherit from this class.
/// </summary>
public abstract class BookingDecorator : IBookingComponent
{
    protected readonly IBookingComponent _wrappedBooking;

    protected BookingDecorator(IBookingComponent booking)
    {
        _wrappedBooking = booking;
    }

    public virtual string GetDescription()
    {
        return _wrappedBooking.GetDescription();
    }

    public virtual decimal GetCost()
    {
        return _wrappedBooking.GetCost();
    }

    public virtual void DisplayDetails()
    {
        _wrappedBooking.DisplayDetails();
    }
}
