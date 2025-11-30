using UniversityBookingSystem.Command;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Factories;
using UniversityBookingSystem.Interfaces;
using UniversityBookingSystem.Observer;
using UniversityBookingSystem.State;

namespace UniversityBookingSystem.Demo;

/// <summary>
/// Demonstrates the three behavioral design patterns:
/// 1. Observer Pattern - Notification system
/// 2. State Pattern - Booking lifecycle management
/// 3. Command Pattern - Operations with undo/redo
/// </summary>
public static class BehavioralPatternsDemo
{
    public static void Run()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║     Behavioral Design Patterns Demonstration              ║");
        Console.WriteLine("║  Observer + State + Command Patterns                      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        Console.WriteLine("Select a demo:");
        Console.WriteLine("1. Observer Pattern Demo");
        Console.WriteLine("2. State Pattern Demo");
        Console.WriteLine("3. Command Pattern Demo");
        Console.WriteLine("4. Complete Integration Demo (All Patterns Together)");
        Console.Write("\n> ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                DemoObserverPattern();
                break;
            case "2":
                DemoStatePattern();
                break;
            case "3":
                DemoCommandPattern();
                break;
            case "4":
                DemoAllPatternsTogether();
                break;
            default:
                DemoAllPatternsTogether();
                break;
        }
    }

    private static void DemoObserverPattern()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("OBSERVER PATTERN DEMO");
        Console.WriteLine("Purpose: Notify multiple observers when booking events occur");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        // Create a booking
        var factory = new TechUniversityFactory();
        var booking = CreateSampleBooking(factory);

        // Create subject (observable)
        var subject = new BookingSubject();

        // Create and attach observers
        var emailObserver = new EmailNotificationObserver("Email Service");
        var loggingObserver = new LoggingObserver();
        var calendarObserver = new CalendarSyncObserver();

        Console.WriteLine("--- Attaching Observers ---");
        subject.Attach(emailObserver);
        subject.Attach(loggingObserver);
        subject.Attach(calendarObserver);

        // Trigger various events
        Console.WriteLine("\n--- Triggering Booking Events ---\n");

        Console.WriteLine("1. Creating Booking:");
        subject.NotifyBookingCreated(booking);

        Console.WriteLine("\n2. Approving Booking:");
        subject.NotifyBookingApproved(booking);

        Console.WriteLine("\n3. Modifying Booking:");
        subject.NotifyBookingModified(booking, "Changed time to 14:00-16:00");

        Console.WriteLine("\n4. Completing Booking:");
        subject.NotifyBookingCompleted(booking);

        // Show logs
        Console.WriteLine();
        loggingObserver.PrintLogs();

        Console.WriteLine("\n--- Detaching Email Observer ---");
        subject.Detach(emailObserver);

        Console.WriteLine("\nCancelling a new booking (Email observer won't be notified):");
        subject.NotifyBookingCancelled(booking);
    }

    private static void DemoStatePattern()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("STATE PATTERN DEMO");
        Console.WriteLine("Purpose: Manage booking lifecycle through different states");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        var factory = new TechUniversityFactory();
        var booking = CreateSampleBooking(factory);

        // Create booking context (starts in Pending state)
        var context = new BookingContext(booking);

        Console.WriteLine("\n--- Valid State Transitions ---\n");

        Console.WriteLine("Initial state:");
        context.DisplayState();

        Console.WriteLine("\n1. Approving booking:");
        context.Approve();
        context.DisplayState();

        Console.WriteLine("\n2. Confirming booking:");
        context.Confirm();
        context.DisplayState();

        Console.WriteLine("\n3. Starting booking:");
        context.Start();
        context.DisplayState();

        Console.WriteLine("\n4. Completing booking:");
        context.Complete();
        context.DisplayState();

        Console.WriteLine("\n--- Invalid State Transitions (Error Handling) ---\n");

        var booking2 = CreateSampleBooking(factory, "LAB-202");
        var context2 = new BookingContext(booking2);

        Console.WriteLine("Trying to start a pending booking (should fail):");
        context2.Start();

        Console.WriteLine("\nTrying to complete a pending booking (should fail):");
        context2.Complete();

        Console.WriteLine("\nApproving and then trying to approve again (should fail):");
        context2.Approve();
        context2.Approve();

        Console.WriteLine("\n--- Cancellation Flow ---\n");
        var booking3 = CreateSampleBooking(factory, "SEM-301");
        var context3 = new BookingContext(booking3);

        Console.WriteLine("Approving booking:");
        context3.Approve();

        Console.WriteLine("\nCancelling approved booking:");
        context3.Cancel();
        context3.DisplayState();

        Console.WriteLine("\nTrying to approve cancelled booking (should fail):");
        context3.Approve();
    }

    private static void DemoCommandPattern()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("COMMAND PATTERN DEMO");
        Console.WriteLine("Purpose: Encapsulate operations with undo/redo capability");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        var factory = new TechUniversityFactory();
        var booking = CreateSampleBooking(factory);
        var context = new BookingContext(booking);

        // Create invoker
        var invoker = new BookingCommandInvoker();

        Console.WriteLine("--- Executing Commands ---\n");

        // Create and execute commands
        var approveCommand = new ApproveBookingCommand(context);
        invoker.ExecuteCommand(approveCommand);

        var confirmCommand = new ConfirmBookingCommand(context);
        invoker.ExecuteCommand(confirmCommand);

        var startCommand = new StartBookingCommand(context);
        invoker.ExecuteCommand(startCommand);

        Console.WriteLine("\nCurrent state:");
        context.DisplayState();

        invoker.ShowHistory();

        Console.WriteLine("\n--- Undo Operations ---\n");

        Console.WriteLine("Undoing last command (Start):");
        invoker.Undo();
        context.DisplayState();

        Console.WriteLine("\nUndoing again (Confirm):");
        invoker.Undo();
        context.DisplayState();

        Console.WriteLine("\n--- Redo Operations ---\n");

        Console.WriteLine("Redoing last undone command (Confirm):");
        invoker.Redo();
        context.DisplayState();

        Console.WriteLine("\nRedoing again (Start):");
        invoker.Redo();
        context.DisplayState();

        Console.WriteLine("\n--- Complete and Try to Undo ---\n");

        var completeCommand = new CompleteBookingCommand(context);
        invoker.ExecuteCommand(completeCommand);
        context.DisplayState();

        Console.WriteLine("\nTrying to redo (should fail - no commands to redo):");
        invoker.Redo();

        invoker.ShowHistory();
    }

    private static void DemoAllPatternsTogether()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("COMPLETE INTEGRATION DEMO");
        Console.WriteLine("All Three Behavioral Patterns Working Together");
        Console.WriteLine("═══════════════════════════════════════════════════════════\n");

        var factory = new TechUniversityFactory();
        var booking = CreateSampleBooking(factory);

        // Set up Observer Pattern
        Console.WriteLine("--- Setting Up Observer Pattern ---");
        var subject = new BookingSubject();
        var emailObserver = new EmailNotificationObserver();
        var loggingObserver = new LoggingObserver();
        var calendarObserver = new CalendarSyncObserver();

        subject.Attach(emailObserver);
        subject.Attach(loggingObserver);
        subject.Attach(calendarObserver);

        // Set up State Pattern with Observer
        Console.WriteLine("\n--- Setting Up State Pattern with Observer Integration ---");
        var context = new BookingContext(booking, subject);

        // Set up Command Pattern
        Console.WriteLine("\n--- Setting Up Command Pattern ---");
        var invoker = new BookingCommandInvoker();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Scenario: Complete Booking Lifecycle                    ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝\n");

        // Initial notification
        subject.NotifyBookingCreated(booking);

        // Execute commands - each triggers state changes and observer notifications
        Console.WriteLine("\n--- Step 1: Approve Booking (Command executes, State changes, Observers notified) ---");
        var approveCmd = new ApproveBookingCommand(context);
        invoker.ExecuteCommand(approveCmd);

        Console.WriteLine("\n--- Step 2: Confirm Booking ---");
        var confirmCmd = new ConfirmBookingCommand(context);
        invoker.ExecuteCommand(confirmCmd);

        Console.WriteLine("\n--- Step 3: Start Booking ---");
        var startCmd = new StartBookingCommand(context);
        invoker.ExecuteCommand(startCmd);

        Console.WriteLine("\n--- Step 4: Complete Booking ---");
        var completeCmd = new CompleteBookingCommand(context);
        invoker.ExecuteCommand(completeCmd);

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Demonstration of Undo Functionality                     ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝\n");

        Console.WriteLine("--- Undoing last two operations ---");
        invoker.Undo();
        invoker.Undo();

        Console.WriteLine("\nCurrent state after undo:");
        context.DisplayState();

        Console.WriteLine("\n--- Redoing operations ---");
        invoker.Redo();
        invoker.Redo();

        Console.WriteLine("\nFinal state:");
        context.DisplayState();

        // Show command history
        Console.WriteLine();
        invoker.ShowHistory();

        // Show event logs
        Console.WriteLine();
        loggingObserver.PrintLogs();

        Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  Pattern Benefits Demonstrated                           ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.WriteLine("\nOBSERVER PATTERN:");
        Console.WriteLine("  ✓ Multiple systems notified automatically (Email, Logs, Calendar)");
        Console.WriteLine("  ✓ Loosely coupled notification system");
        Console.WriteLine("  ✓ Easy to add/remove observers at runtime");

        Console.WriteLine("\nSTATE PATTERN:");
        Console.WriteLine("  ✓ Clean state transitions with validation");
        Console.WriteLine("  ✓ State-specific behavior encapsulated");
        Console.WriteLine("  ✓ Prevents invalid operations based on current state");

        Console.WriteLine("\nCOMMAND PATTERN:");
        Console.WriteLine("  ✓ Operations encapsulated as objects");
        Console.WriteLine("  ✓ Full undo/redo capability");
        Console.WriteLine("  ✓ Command history tracking");
        Console.WriteLine("  ✓ Decouples operation invocation from execution");
    }

    private static Booking CreateSampleBooking(IUniversityFactory factory, string roomCode = "A101")
    {
        var room = factory.CreateLectureHall(roomCode);
        var layout = new RoomLayout
        {
            Name = "Conference Layout",
            Seats = 100,
            Equipment = { "Projector", "Microphone" }
        };

        return new Booking
        {
            Room = room,
            Layout = layout,
            Start = DateTime.Now.AddHours(2),
            End = DateTime.Now.AddHours(4),
            RequestedBy = "Dr. Smith",
            Approved = false
        };
    }
}
