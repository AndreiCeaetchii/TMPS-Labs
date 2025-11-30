namespace UniversityBookingSystem.State;

public class ApprovedState : IBookingState
{
    public string StateName => "Approved";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already approved.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot reject an already approved booking. Cancel it instead.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking confirmed!");
        context.CurrentState = new ConfirmedState();
        context.Subject?.NotifyBookingModified(context.Booking, "Booking has been confirmed");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot start booking. Please confirm first.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot complete an approved booking. Need to confirm and start first.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking cancelled.");
        context.CurrentState = new CancelledState();
        context.Subject?.NotifyBookingCancelled(context.Booking, "Approved booking was cancelled");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Ready for confirmation");
    }
}
