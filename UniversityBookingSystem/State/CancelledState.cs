namespace UniversityBookingSystem.State;

public class CancelledState : IBookingState
{
    public string StateName => "Cancelled";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot approve a cancelled booking.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already cancelled.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot confirm a cancelled booking.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot start a cancelled booking.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot complete a cancelled booking.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already cancelled.");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Booking was cancelled");
    }
}
