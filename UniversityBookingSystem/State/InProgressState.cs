namespace UniversityBookingSystem.State;

public class InProgressState : IBookingState
{
    public string StateName => "InProgress";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already in progress.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot reject a booking that is in progress.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already in progress.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already in progress.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking completed successfully!");
        context.CurrentState = new CompletedState();
        context.Subject?.NotifyBookingCompleted(context.Booking, "Booking event has been completed");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot cancel a booking in progress. Complete it or wait for it to finish.");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Event is happening now");
    }
}
