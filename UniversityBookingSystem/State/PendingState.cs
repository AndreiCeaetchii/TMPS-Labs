namespace UniversityBookingSystem.State;

public class PendingState : IBookingState
{
    public string StateName => "Pending";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking approved!");
        context.CurrentState = new ApprovedState();
        context.Subject?.NotifyBookingApproved(context.Booking, "Booking has been approved");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking rejected!");
        context.CurrentState = new RejectedState();
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot confirm a pending booking. Please approve first.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot start a pending booking. Please approve and confirm first.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot complete a pending booking.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking cancelled from pending state.");
        context.CurrentState = new CancelledState();
        context.Subject?.NotifyBookingCancelled(context.Booking, "Booking cancelled while pending");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Awaiting approval");
    }
}
