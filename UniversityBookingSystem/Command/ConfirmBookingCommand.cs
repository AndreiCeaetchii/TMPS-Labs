using UniversityBookingSystem.State;

namespace UniversityBookingSystem.Command;

public class ConfirmBookingCommand : IBookingCommand
{
    private readonly BookingContext _bookingContext;
    private string? _previousState;

    public ConfirmBookingCommand(BookingContext bookingContext)
    {
        _bookingContext = bookingContext;
    }

    public string CommandName => "Confirm Booking";

    public bool CanUndo => true;

    public void Execute()
    {
        _previousState = _bookingContext.GetCurrentStateName();
        Console.WriteLine($"\n[COMMAND] Executing: {CommandName}");
        _bookingContext.Confirm();
    }

    public void Undo()
    {
        if (_previousState == null)
        {
            Console.WriteLine($"[COMMAND] Cannot undo {CommandName}: no previous state recorded");
            return;
        }

        Console.WriteLine($"\n[COMMAND] Undoing: {CommandName} (reverting to {_previousState})");

        _bookingContext.CurrentState = _previousState switch
        {
            "Pending" => new PendingState(),
            "Approved" => new ApprovedState(),
            "Confirmed" => new ConfirmedState(),
            "InProgress" => new InProgressState(),
            "Completed" => new CompletedState(),
            "Cancelled" => new CancelledState(),
            "Rejected" => new RejectedState(),
            _ => new PendingState()
        };
    }
}
