using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Observer;

namespace UniversityBookingSystem.State;

public class BookingContext
{
    private IBookingState _currentState;
    public Booking Booking { get; }
    public BookingSubject? Subject { get; set; }

    public BookingContext(Booking booking, BookingSubject? subject = null)
    {
        Booking = booking;
        Subject = subject;
        _currentState = new PendingState();
        Console.WriteLine($"[CONTEXT] Booking created in {_currentState.StateName} state");
    }

    public IBookingState CurrentState
    {
        get => _currentState;
        set
        {
            var previousState = _currentState.StateName;
            _currentState = value;
            Console.WriteLine($"[STATE TRANSITION] {previousState} -> {_currentState.StateName}");
        }
    }

    public void Approve() => _currentState.Approve(this);
    public void Reject() => _currentState.Reject(this);
    public void Confirm() => _currentState.Confirm(this);
    public void Start() => _currentState.Start(this);
    public void Complete() => _currentState.Complete(this);
    public void Cancel() => _currentState.Cancel(this);

    public void DisplayState() => _currentState.DisplayState();

    public string GetCurrentStateName() => _currentState.StateName;
}
