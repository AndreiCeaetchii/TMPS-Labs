# Behavioral Design Patterns Implementation Report

## Table of Contents
- [Overview](#overview)
- [Behavioral Design Patterns Implemented](#behavioral-design-patterns-implemented)
  - [1. Observer Pattern](#1-observer-pattern)
  - [2. State Pattern](#2-state-pattern)
  - [3. Command Pattern](#3-command-pattern)
- [Pattern Interactions](#pattern-interactions)
- [Architecture](#architecture)
- [Code Examples](#code-examples)
- [Benefits](#benefits)
- [Conclusion](#conclusion)

---

## Overview

This report demonstrates the implementation of **three behavioral design patterns** in the University Booking System. Behavioral design patterns are concerned with algorithms and the assignment of responsibilities between objects. They help make the system more flexible in carrying out communication between objects.

The three patterns implemented are:
1. **Observer Pattern** - Defines a one-to-many dependency between objects for event notification
2. **State Pattern** - Allows an object to alter its behavior when its internal state changes
3. **Command Pattern** - Encapsulates requests as objects, enabling parameterization and queuing

These patterns complement the previously implemented creational patterns (Abstract Factory, Builder, Singleton, Director) and structural patterns (Decorator, Composite, Facade) to create a comprehensive, professional booking system.

---

## Behavioral Design Patterns Implemented

### 1. Observer Pattern

#### Purpose
The Observer pattern defines a one-to-many dependency between objects so that when one object (subject) changes state, all its dependents (observers) are notified and updated automatically. This pattern is essential for implementing distributed event handling systems.

#### Implementation

**Observer Interface:**
```csharp
public interface IBookingObserver
{
    void OnBookingCreated(BookingEventArgs args);
    void OnBookingApproved(BookingEventArgs args);
    void OnBookingCancelled(BookingEventArgs args);
    void OnBookingModified(BookingEventArgs args);
    void OnBookingCompleted(BookingEventArgs args);
}
```

**Event Arguments:**
```csharp
public class BookingEventArgs
{
    public Booking Booking { get; set; }
    public string EventType { get; set; }
    public DateTime EventTime { get; set; }
    public string Message { get; set; }
    public string? PreviousState { get; set; }
    public string? NewState { get; set; }
}
```

**Subject (Observable):**
```csharp
public class BookingSubject
{
    private readonly List<IBookingObserver> _observers = new();

    public void Attach(IBookingObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Detach(IBookingObserver observer)
    {
        _observers.Remove(observer);
    }

    public void NotifyBookingCreated(Booking booking, string message = "")
    {
        var args = new BookingEventArgs(booking, "Created", message);
        foreach (var observer in _observers)
        {
            observer.OnBookingCreated(args);
        }
    }

    // Similar methods for other events...
}
```

#### Concrete Observers

**1. EmailNotificationObserver**
Sends email notifications when booking events occur.
```csharp
public class EmailNotificationObserver : IBookingObserver
{
    public void OnBookingCreated(BookingEventArgs args)
    {
        SendEmail($"New Booking Created: {args.Booking}");
        LogNotification(args, "CREATED");
    }

    private void SendEmail(string content)
    {
        Console.WriteLine($"  [EMAIL] Sending notification: {content}");
    }
}
```

**2. LoggingObserver**
Maintains a persistent log of all booking events.
```csharp
public class LoggingObserver : IBookingObserver
{
    private readonly List<string> _logEntries = new();

    public void OnBookingCreated(BookingEventArgs args)
    {
        var logEntry = $"[{args.EventTime}] BOOKING_CREATED: {args.Message}";
        _logEntries.Add(logEntry);
        Console.WriteLine($"  [LOG] {logEntry}");
    }

    public IReadOnlyList<string> GetLogs() => _logEntries.AsReadOnly();
}
```

**3. CalendarSyncObserver**
Synchronizes booking events with calendar systems.
```csharp
public class CalendarSyncObserver : IBookingObserver
{
    private readonly Dictionary<string, string> _calendarEntries = new();

    public void OnBookingCreated(BookingEventArgs args)
    {
        var calendarId = AddToCalendar(args.Booking);
        Console.WriteLine($"  [CALENDAR] Added to calendar: {calendarId}");
    }

    public void OnBookingCancelled(BookingEventArgs args)
    {
        RemoveFromCalendar(args.Booking);
        Console.WriteLine($"  [CALENDAR] Removed from calendar");
    }
}
```

#### Usage Example
```csharp
// Create subject
var subject = new BookingSubject();

// Create and attach observers
var emailObserver = new EmailNotificationObserver();
var loggingObserver = new LoggingObserver();
var calendarObserver = new CalendarSyncObserver();

subject.Attach(emailObserver);
subject.Attach(loggingObserver);
subject.Attach(calendarObserver);

// Trigger events - all observers are notified automatically
subject.NotifyBookingCreated(booking);
subject.NotifyBookingApproved(booking);
subject.NotifyBookingCompleted(booking);
```

#### Benefits
- **Loose Coupling**: Subject doesn't need to know concrete observer classes
- **Dynamic Relationships**: Observers can be added/removed at runtime
- **Broadcast Communication**: One event notifies multiple observers
- **Open/Closed Principle**: Easy to add new observers without modifying subject
- **Single Responsibility**: Each observer handles one type of notification

#### File Locations
- `Observer/IBookingObserver.cs` - Observer interface
- `Observer/BookingEventArgs.cs` - Event data
- `Observer/BookingSubject.cs` - Subject (observable)
- `Observer/EmailNotificationObserver.cs` - Email notifications
- `Observer/LoggingObserver.cs` - Event logging
- `Observer/CalendarSyncObserver.cs` - Calendar synchronization

---

### 2. State Pattern

#### Purpose
The State pattern allows an object to alter its behavior when its internal state changes. The object will appear to change its class. This pattern is ideal for objects that must change their behavior at runtime depending on their state.

#### Implementation

**State Interface:**
```csharp
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
```

**Context Class:**
```csharp
public class BookingContext
{
    private IBookingState _currentState;
    public Booking Booking { get; }
    public BookingSubject? Subject { get; set; }

    public BookingContext(Booking booking, BookingSubject? subject = null)
    {
        Booking = booking;
        Subject = subject;
        _currentState = new PendingState(); // Initial state
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

    // Delegate behavior to current state
    public void Approve() => _currentState.Approve(this);
    public void Confirm() => _currentState.Confirm(this);
    public void Start() => _currentState.Start(this);
    public void Complete() => _currentState.Complete(this);
    public void Cancel() => _currentState.Cancel(this);
}
```

#### State Lifecycle

```
Pending → Approved → Confirmed → InProgress → Completed
    ↓         ↓          ↓
Cancelled  Cancelled  Cancelled

Pending → Rejected
```

#### Concrete States

**1. PendingState** - Initial state
```csharp
public class PendingState : IBookingState
{
    public string StateName => "Pending";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking approved!");
        context.CurrentState = new ApprovedState();
        context.Subject?.NotifyBookingApproved(context.Booking);
    }

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot start a pending booking.");
    }

    public void Cancel(BookingContext context)
    {
        context.CurrentState = new CancelledState();
        context.Subject?.NotifyBookingCancelled(context.Booking);
    }
}
```

**2. ApprovedState** - Approved but not confirmed
```csharp
public class ApprovedState : IBookingState
{
    public string StateName => "Approved";

    public void Confirm(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking confirmed!");
        context.CurrentState = new ConfirmedState();
    }

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already approved.");
    }
}
```

**3. ConfirmedState** - Ready to start
```csharp
public class ConfirmedState : IBookingState
{
    public string StateName => "Confirmed";

    public void Start(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking started!");
        context.CurrentState = new InProgressState();
    }
}
```

**4. InProgressState** - Event is happening
```csharp
public class InProgressState : IBookingState
{
    public string StateName => "InProgress";

    public void Complete(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking completed!");
        context.CurrentState = new CompletedState();
        context.Subject?.NotifyBookingCompleted(context.Booking);
    }

    public void Cancel(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Cannot cancel a booking in progress.");
    }
}
```

**5. CompletedState** - Terminal state
```csharp
public class CompletedState : IBookingState
{
    public string StateName => "Completed";

    public void Approve(BookingContext context)
    {
        Console.WriteLine($"[{StateName}] Booking is already completed.");
    }

    // All other operations are rejected in this terminal state
}
```

**6. CancelledState** - Terminal state
**7. RejectedState** - Terminal state

#### Usage Example
```csharp
var booking = CreateBooking();
var context = new BookingContext(booking);

// State-specific behavior
context.Approve();   // Pending → Approved
context.Confirm();   // Approved → Confirmed
context.Start();     // Confirmed → InProgress
context.Complete();  // InProgress → Completed

// Invalid transitions are handled gracefully
context.Approve();   // "Booking is already completed."
```

#### Benefits
- **Eliminates Conditional Logic**: No large if/else or switch statements
- **Single Responsibility**: Each state class handles one state's behavior
- **Easy to Add States**: New states don't affect existing ones
- **State Transitions**: Centralized and explicit
- **Prevents Invalid Operations**: Each state defines valid operations

#### File Locations
- `State/IBookingState.cs` - State interface
- `State/BookingContext.cs` - Context managing state
- `State/PendingState.cs` - Initial state
- `State/ApprovedState.cs` - Approved state
- `State/ConfirmedState.cs` - Confirmed state
- `State/InProgressState.cs` - Active state
- `State/CompletedState.cs` - Terminal state (success)
- `State/CancelledState.cs` - Terminal state (cancelled)
- `State/RejectedState.cs` - Terminal state (rejected)

---

### 3. Command Pattern

#### Purpose
The Command pattern encapsulates a request as an object, thereby letting you parameterize clients with different requests, queue or log requests, and support undoable operations. This pattern is essential for implementing undo/redo functionality and transaction management.

#### Implementation

**Command Interface:**
```csharp
public interface IBookingCommand
{
    string CommandName { get; }
    void Execute();
    void Undo();
    bool CanUndo { get; }
}
```

**Invoker Class:**
```csharp
public class BookingCommandInvoker
{
    private readonly Stack<IBookingCommand> _commandHistory = new();
    private readonly Stack<IBookingCommand> _undoneCommands = new();

    public void ExecuteCommand(IBookingCommand command)
    {
        command.Execute();
        _commandHistory.Push(command);
        _undoneCommands.Clear(); // Clear redo history
        Console.WriteLine($"[INVOKER] Command executed: {command.CommandName}");
    }

    public void Undo()
    {
        if (_commandHistory.Count == 0) return;

        var command = _commandHistory.Pop();
        if (command.CanUndo)
        {
            command.Undo();
            _undoneCommands.Push(command);
        }
    }

    public void Redo()
    {
        if (_undoneCommands.Count == 0) return;

        var command = _undoneCommands.Pop();
        command.Execute();
        _commandHistory.Push(command);
    }
}
```

#### Concrete Commands

**1. ApproveBookingCommand**
```csharp
public class ApproveBookingCommand : IBookingCommand
{
    private readonly BookingContext _bookingContext;
    private string? _previousState;

    public string CommandName => "Approve Booking";
    public bool CanUndo => true;

    public void Execute()
    {
        _previousState = _bookingContext.GetCurrentStateName();
        Console.WriteLine($"\n[COMMAND] Executing: {CommandName}");
        _bookingContext.Approve();
    }

    public void Undo()
    {
        Console.WriteLine($"\n[COMMAND] Undoing: {CommandName}");
        // Restore previous state
        _bookingContext.CurrentState = RestoreState(_previousState);
    }
}
```

**2. ConfirmBookingCommand**
Confirms an approved booking.

**3. StartBookingCommand**
Starts a confirmed booking event.

**4. CompleteBookingCommand**
Marks a booking as completed.

**5. CancelBookingCommand**
Cancels a booking.

**6. RejectBookingCommand**
Rejects a booking request.

#### Usage Example
```csharp
var context = new BookingContext(booking);
var invoker = new BookingCommandInvoker();

// Execute commands
var approveCmd = new ApproveBookingCommand(context);
invoker.ExecuteCommand(approveCmd);

var confirmCmd = new ConfirmBookingCommand(context);
invoker.ExecuteCommand(confirmCmd);

var startCmd = new StartBookingCommand(context);
invoker.ExecuteCommand(startCmd);

// Undo operations
invoker.Undo();  // Reverts Start
invoker.Undo();  // Reverts Confirm

// Redo operations
invoker.Redo();  // Re-executes Confirm
invoker.Redo();  // Re-executes Start

// View history
invoker.ShowHistory();
```

#### Benefits
- **Undo/Redo Support**: Full reversibility of operations
- **Command History**: Track all executed operations
- **Decoupling**: Invoker doesn't know command implementation details
- **Queuing**: Commands can be queued for later execution
- **Macro Commands**: Combine multiple commands into one
- **Logging**: Easy to log all operations
- **Parameterization**: Different commands can be passed to invoker

#### File Locations
- `Command/IBookingCommand.cs` - Command interface
- `Command/BookingCommandInvoker.cs` - Invoker with undo/redo
- `Command/ApproveBookingCommand.cs` - Approve operation
- `Command/ConfirmBookingCommand.cs` - Confirm operation
- `Command/StartBookingCommand.cs` - Start operation
- `Command/CompleteBookingCommand.cs` - Complete operation
- `Command/CancelBookingCommand.cs` - Cancel operation
- `Command/RejectBookingCommand.cs` - Reject operation

---

## Pattern Interactions

### How the Three Patterns Work Together

```
┌─────────────────────────────────────────────────────────┐
│              COMMAND PATTERN (Invoker)                  │
│         Executes operations with undo/redo              │
└────────────────────┬────────────────────────────────────┘
                     │
                     ↓
┌─────────────────────────────────────────────────────────┐
│              STATE PATTERN (Context)                    │
│         Manages booking lifecycle states                │
└────────────────────┬────────────────────────────────────┘
                     │
                     ↓
┌─────────────────────────────────────────────────────────┐
│            OBSERVER PATTERN (Subject)                   │
│         Notifies observers of state changes             │
└────────────────────┬────────────────────────────────────┘
                     │
        ┌────────────┼────────────┐
        ↓            ↓            ↓
   [Email]      [Logging]   [Calendar]
  Observer      Observer     Observer
```

### Integration Flow

1. **Command Executes** → Changes state via Command Pattern
2. **State Changes** → Managed by State Pattern with validation
3. **State Transition** → Triggers Observer notifications
4. **Observers Notified** → Email, logs, calendar sync automatically
5. **Command History** → Enables undo/redo of entire flow

### Example: Complete Workflow
```csharp
// Setup all three patterns
var subject = new BookingSubject();
subject.Attach(new EmailNotificationObserver());
subject.Attach(new LoggingObserver());

var context = new BookingContext(booking, subject);
var invoker = new BookingCommandInvoker();

// Execute command (Command Pattern)
var approveCmd = new ApproveBookingCommand(context);
invoker.ExecuteCommand(approveCmd);
// ↓
// State changes from Pending to Approved (State Pattern)
// ↓
// All observers notified of approval (Observer Pattern)
// ↓
// Command saved in history for undo (Command Pattern)

// Undo the approval
invoker.Undo();
// ↓
// State reverts to Pending (State Pattern)
// ↓
// Could notify observers of state reversion
```

---

## Architecture

### Updated System Structure

```
UniversityBookingSystem/
├── Entities/               # Domain entities
├── Interfaces/             # Contracts and abstractions
├── Factories/              # Abstract Factory pattern
├── Services/               # Builder, Director, Singleton
├── Decorators/             # Decorator pattern
├── Composite/              # Composite pattern
├── Facade/                 # Facade pattern
│
├── Observer/               # Observer pattern ⭐ NEW
│   ├── IBookingObserver.cs
│   ├── BookingEventArgs.cs
│   ├── BookingSubject.cs
│   ├── EmailNotificationObserver.cs
│   ├── LoggingObserver.cs
│   └── CalendarSyncObserver.cs
│
├── State/                  # State pattern ⭐ NEW
│   ├── IBookingState.cs
│   ├── BookingContext.cs
│   ├── PendingState.cs
│   ├── ApprovedState.cs
│   ├── ConfirmedState.cs
│   ├── InProgressState.cs
│   ├── CompletedState.cs
│   ├── CancelledState.cs
│   └── RejectedState.cs
│
├── Command/                # Command pattern ⭐ NEW
│   ├── IBookingCommand.cs
│   ├── BookingCommandInvoker.cs
│   ├── ApproveBookingCommand.cs
│   ├── ConfirmBookingCommand.cs
│   ├── StartBookingCommand.cs
│   ├── CompleteBookingCommand.cs
│   ├── CancelBookingCommand.cs
│   └── RejectBookingCommand.cs
│
├── Demo/
│   ├── ConsoleUI.cs (Creational patterns)
│   ├── StructuralPatternsDemo.cs (Structural patterns)
│   └── BehavioralPatternsDemo.cs (Behavioral patterns) ⭐ NEW
│
└── Program.cs
```

---

## Code Examples

### Example 1: Observer Pattern
```csharp
// Create observers
var emailObserver = new EmailNotificationObserver();
var loggingObserver = new LoggingObserver();

// Create subject and attach observers
var subject = new BookingSubject();
subject.Attach(emailObserver);
subject.Attach(loggingObserver);

// Notify all observers
subject.NotifyBookingCreated(booking);
// Output:
//   [EMAIL] Sending notification...
//   [LOG] [2025-01-15 10:00:00] BOOKING_CREATED: ...
```

### Example 2: State Pattern
```csharp
var context = new BookingContext(booking);

context.Approve();   // Pending → Approved ✓
context.Confirm();   // Approved → Confirmed ✓
context.Start();     // Confirmed → InProgress ✓
context.Complete();  // InProgress → Completed ✓

// Invalid operation handled gracefully
context.Start();     // "Booking is already completed." ✗
```

### Example 3: Command Pattern
```csharp
var invoker = new BookingCommandInvoker();

// Execute commands
invoker.ExecuteCommand(new ApproveBookingCommand(context));
invoker.ExecuteCommand(new ConfirmBookingCommand(context));
invoker.ExecuteCommand(new StartBookingCommand(context));

// Undo last two operations
invoker.Undo();  // Reverts Start
invoker.Undo();  // Reverts Confirm

// Redo operations
invoker.Redo();  // Re-executes Confirm
```

### Example 4: All Patterns Together
```csharp
// Setup
var subject = new BookingSubject();
subject.Attach(new EmailNotificationObserver());
subject.Attach(new LoggingObserver());
subject.Attach(new CalendarSyncObserver());

var context = new BookingContext(booking, subject);
var invoker = new BookingCommandInvoker();

// Complete workflow with all patterns
invoker.ExecuteCommand(new ApproveBookingCommand(context));
// → Command executes
// → State: Pending → Approved
// → Observers: Email sent, logged, calendar updated

invoker.ExecuteCommand(new ConfirmBookingCommand(context));
// → State: Approved → Confirmed
// → Observers: All notified

invoker.Undo();
// → State: Confirmed → Approved
// → Command history maintained

invoker.ShowHistory();
// → Displays all executed commands
```

---

## Benefits

### Observer Pattern Benefits
✅ **Loose Coupling**: Subject and observers are independently reusable
✅ **Dynamic Relationships**: Add/remove observers at runtime
✅ **Broadcast Communication**: One-to-many notification automatically
✅ **Open/Closed Principle**: Add new observers without modifying subject
✅ **Event-Driven Architecture**: Natural fit for event-driven systems

### State Pattern Benefits
✅ **Eliminates Conditionals**: No complex if/else chains
✅ **Single Responsibility**: Each state handles its own behavior
✅ **Easy Extension**: Add new states without modifying existing ones
✅ **Explicit State Transitions**: Clear and traceable state changes
✅ **Prevents Invalid Operations**: State-specific validation

### Command Pattern Benefits
✅ **Undo/Redo Support**: Full reversibility of operations
✅ **Command History**: Complete audit trail
✅ **Decoupling**: Invoker independent of concrete commands
✅ **Queuing**: Commands can be queued and executed later
✅ **Macro Commands**: Combine multiple operations
✅ **Logging & Auditing**: Track all system operations

### Combined Benefits
🎯 **Complete Lifecycle Management**: State pattern handles booking states
🎯 **Automatic Notifications**: Observer pattern for event distribution
🎯 **Operation Control**: Command pattern for undo/redo
🎯 **Maintainability**: Clear separation of concerns
🎯 **Extensibility**: Easy to add new states, commands, or observers
🎯 **Testability**: Each pattern can be tested independently
🎯 **Professional Architecture**: Industry-standard patterns working together

---

## Conclusion

This implementation demonstrates how **three behavioral design patterns** work together to create a sophisticated booking lifecycle management system:

### Observer Pattern
- Provides automatic notification system for booking events
- Enables loose coupling between booking system and notification services
- Supports multiple observers (email, logging, calendar) simultaneously

### State Pattern
- Manages complex booking lifecycle with clear state transitions
- Eliminates conditional logic and prevents invalid operations
- Makes state-specific behavior explicit and maintainable

### Command Pattern
- Encapsulates operations as objects with full undo/redo support
- Provides command history for auditing and debugging
- Decouples operation invocation from execution

### Integration
These patterns complement each other perfectly:
- **Commands** execute operations that change **States**
- **State changes** trigger **Observer** notifications
- **Observers** react to state changes automatically
- **Commands** maintain history for undo/redo functionality

Together with the previously implemented patterns (Creational and Structural), the system now demonstrates:
- ✅ **10 Design Patterns** total across all categories
- ✅ **Production-ready architecture** with separation of concerns
- ✅ **SOLID principles** applied throughout
- ✅ **Comprehensive event handling** with full lifecycle management
- ✅ **Professional code quality** suitable for enterprise applications

---

## References

- Gang of Four (GoF) Design Patterns
- Head First Design Patterns
- Refactoring Guru - Design Patterns
- Microsoft C# Documentation
- Clean Code: A Handbook of Agile Software Craftsmanship
