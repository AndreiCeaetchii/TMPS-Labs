namespace UniversityBookingSystem.State;

public interface IBookingState
{
    string StateName { get; }

    void Approve(BookingContext context);
    void Reject(BookingContext context);
    void Confirm(BookingContext context);
    void Start(BookingContext context);
    void Complete(BookingContext context);
    void Cancel(BookingContext context);

    void DisplayState();
}
