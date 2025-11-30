namespace UniversityBookingSystem.State;

public class CompletedState : IBookingState
{
    public string StateName => "Completed";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already completed.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot reject a completed booking.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already completed.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already completed.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already completed.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot cancel a completed booking.");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Event has finished");
    }
}
