namespace UniversityBookingSystem.State;

public class ConfirmedState : IBookingState
{
    public string StateName => "Confirmed";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already approved and confirmed.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot reject a confirmed booking. Cancel it instead.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already confirmed.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking started! Event is now in progress.");
        context.CurrentState = new InProgressState();
        context.Subject?.NotifyBookingModified(context.Booking, "Booking event has started");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot complete booking. Need to start it first.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Confirmed booking cancelled.");
        context.CurrentState = new CancelledState();
        context.Subject?.NotifyBookingCancelled(context.Booking, "Confirmed booking was cancelled");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Ready to start");
    }
}
