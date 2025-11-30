namespace UniversityBookingSystem.State;

public class RejectedState : IBookingState
{
    public string StateName => "Rejected";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot approve a rejected booking. Create a new booking instead.");
    }

    public void Reject(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already rejected.");
    }

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot confirm a rejected booking.");
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot start a rejected booking.");
    }

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot complete a rejected booking.");
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already rejected.");
    }

    public void DisplayState()
    {
        Console.WriteLine($"Current State: {StateName} - Booking was rejected");
    }
}
