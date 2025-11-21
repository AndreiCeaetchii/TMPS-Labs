using UniversityBookingSystem.Composite;
using UniversityBookingSystem.Decorators;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Facade;
using UniversityBookingSystem.Factories;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Demo;

public static class StructuralPatternsDemo
{
    public static void Run()
    {
        Console.Clear();

        // Ask user which demo to run
        Console.WriteLine("Select a demonstration:");
        Console.WriteLine("1. Decorator Pattern Demo (Adding services to bookings)");
        Console.WriteLine("2. Composite Pattern Demo (Room groupings)");
        Console.WriteLine("3. Facade Pattern Demo (Simplified booking interface)");
        Console.WriteLine("4. Complete Demo (All patterns together)");
        Console.Write("\n> ");

        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                DemonstrateDecoratorPattern();
                break;
            case "2":
                DemonstrateCompositePattern();
                break;
            case "3":
                DemonstrateFacadePattern();
                break;
            case "4":
                DemonstrateAllPatterns();
                break;
            default:
                Console.WriteLine("Invalid choice. Running complete demo...");
                DemonstrateAllPatterns();
                break;
        }

        Console.WriteLine("\n\nPress any key to exit...");
        Console.ReadKey();
    }

    private static void DemonstrateDecoratorPattern()
    {
        Console.WriteLine("\nThe Decorator pattern allows adding responsibilities to objects");
        Console.WriteLine("dynamically without modifying their structure.\n");

        // Create a base booking
        var factory = new TechUniversityFactory();
        var room = factory.CreateLectureHall("A101");
        var layout = new RoomLayout { Name = "Conference Layout", Seats = 150 };

        var baseBooking = new Booking
        {
            Room = room,
            Layout = layout,
            Start = DateTime.Now.AddHours(2),
            End = DateTime.Now.AddHours(4),
            RequestedBy = "Dr. Smith",
            Approved = true
        };

        // Start with base component
        IBookingComponent booking = new BookingComponent(baseBooking);

        Console.WriteLine("--- Base Booking ---");
        booking.DisplayDetails();
        Console.WriteLine($"Total Cost: ${booking.GetCost():F2}\n");

        // Add catering
        Console.WriteLine("\n--- Adding Catering Service ---");
        booking = new CateringDecorator(booking, "Premium Lunch", 100);
        booking.DisplayDetails();
        Console.WriteLine($"Total Cost: ${booking.GetCost():F2}\n");

        // Add equipment
        Console.WriteLine("\n--- Adding Equipment ---");
        booking = new EquipmentDecorator(booking, "Projector", "Microphone", "Whiteboard");
        booking.DisplayDetails();
        Console.WriteLine($"Total Cost: ${booking.GetCost():F2}\n");

        // Add recording
        Console.WriteLine("\n--- Adding Recording & Streaming ---");
        booking = new RecordingDecorator(booking, recording: true, liveStreaming: true);
        booking.DisplayDetails();
        Console.WriteLine($"Total Cost: ${booking.GetCost():F2}\n");

        // Add setup
        Console.WriteLine("\n--- Adding Theater Setup ---");
        booking = new SetupDecorator(booking, "Theater");
        booking.DisplayDetails();
        Console.WriteLine($"FINAL TOTAL COST: ${booking.GetCost():F2}");
    }

    private static void DemonstrateCompositePattern()
    {
        Console.WriteLine("\nThe Composite pattern allows treating individual objects and");
        Console.WriteLine("compositions of objects uniformly.\n");

        var factory = new TechUniversityFactory();

        // Create individual rooms (Leaf nodes)
        var mainHall = new RoomLeaf(factory.CreateLectureHall("MH-100"));
        var lab1 = new RoomLeaf(factory.CreateLab("LAB-201"));
        var lab2 = new RoomLeaf(factory.CreateLab("LAB-202"));
        var seminar1 = new RoomLeaf(factory.CreateSeminarRoom("SEM-301"));
        var seminar2 = new RoomLeaf(factory.CreateSeminarRoom("SEM-302"));

        Console.WriteLine("--- Individual Rooms Created ---");
        mainHall.DisplayStructure();
        lab1.DisplayStructure();
        lab2.DisplayStructure();
        seminar1.DisplayStructure();
        seminar2.DisplayStructure();

        // Create a lab wing (Composite)
        var labWing = new RoomGroup("Computer Science Lab Wing");
        labWing.Add(lab1);
        labWing.Add(lab2);

        Console.WriteLine("\n--- Lab Wing Group (Composite) ---");
        labWing.DisplayStructure();

        // Create a seminar floor (Composite)
        var seminarFloor = new RoomGroup("3rd Floor Seminar Rooms");
        seminarFloor.Add(seminar1);
        seminarFloor.Add(seminar2);

        Console.WriteLine("\n--- Seminar Floor Group (Composite) ---");
        seminarFloor.DisplayStructure();

        // Create a complete conference center (Composite of Composites)
        var conferenceCenter = new RoomGroup("University Conference Center");
        conferenceCenter.Add(mainHall);
        conferenceCenter.Add(labWing);
        conferenceCenter.Add(seminarFloor);

        Console.WriteLine("\n--- Complete Conference Center Hierarchy ---");
        conferenceCenter.DisplayStructure();

        Console.WriteLine($"Total Capacity: {conferenceCenter.GetTotalCapacity()} people                    ║");
        Console.WriteLine($"Total Rooms: {conferenceCenter.GetAllRoomCodes().Count}                               ║");

        // Demonstrate availability check
        var start = DateTime.Now.AddHours(3);
        var end = DateTime.Now.AddHours(5);
        Console.WriteLine($"\nChecking availability from {start:t} to {end:t}...");
        Console.WriteLine($"Conference Center Available: {conferenceCenter.IsAvailable(start, end)}");
    }

    private static void DemonstrateFacadePattern()
    {
        Console.WriteLine("\nThe Facade pattern provides a simplified interface to a");
        Console.WriteLine("complex subsystem, hiding its complexity from clients.\n");

        Console.WriteLine("Choose University Type:");
        Console.WriteLine("1. Technical University");
        Console.WriteLine("2. Liberal Arts University");
        Console.WriteLine("3. Medical University");
        Console.Write("> ");

        var choice = Console.ReadLine();
        var universityType = choice switch
        {
            "1" => UniversityType.Technical,
            "2" => UniversityType.LiberalArts,
            "3" => UniversityType.Medical,
            _ => UniversityType.Technical
        };

        // Create facade - hides all complexity!
        var facade = new BookingSystemFacade(universityType);

        Console.WriteLine("\n--- Example 1: Simple Booking (Facade hides Builder, Factory) ---");
        var simpleBooking = facade.CreateSimpleBooking(
            "A205",
            RoomTypeEnum.LectureHall,
            DateTime.Now.AddHours(2),
            DateTime.Now.AddHours(4),
            "Prof. Johnson");
        Console.WriteLine(simpleBooking);

        Console.WriteLine("\n--- Example 2: Template Booking (Facade hides Director) ---");
        var examBooking = facade.CreateExamBooking("EXAM-101");
        Console.WriteLine(examBooking);

        Console.WriteLine("\n--- Example 3: Enhanced Booking (Facade hides Decorators) ---");
        var enhancedBooking = BookingSystemFacade.CreateEnhancedBooking(
            simpleBooking,
            includeCatering: true,
            cateringPeople: 80,
            cateringType: "Continental Breakfast",
            includeRecording: true,
            equipment: new[] { "Video Conferencing", "Sound System" },
            setupType: "U-Shape");

        enhancedBooking.DisplayDetails();
        Console.WriteLine($"\nTotal Cost with Enhancements: ${enhancedBooking.GetCost():F2}");

        Console.WriteLine("\n--- Example 4: Premium Conference (One method does it all!) ---");
        var premiumBooking = facade.CreatePremiumConferenceBooking(
            "PREMIUM-500",
            DateTime.Now.AddDays(7),
            DateTime.Now.AddDays(7).AddHours(8),
            "Conference Committee",
            200);

        premiumBooking.DisplayDetails();
        Console.WriteLine($"PREMIUM PACKAGE COST: ${premiumBooking.GetCost():F2}");

        Console.WriteLine("\n--- Example 5: Room Group Creation (Facade hides Composite) ---");
        var roomGroup = facade.CreateConferenceRoomGroup(
            "Annual Symposium Venue",
            "HALL-A", "HALL-B", "HALL-C", "LAB-1", "LAB-2", "SEM-1");

        roomGroup.DisplayStructure();
        Console.WriteLine($"\nTotal Group Capacity: {roomGroup.GetTotalCapacity()} people");

        Console.WriteLine("\n--- All Bookings in System ---");
        facade.DisplayAllBookings();
    }

    private static void DemonstrateAllPatterns()
    { 
        Console.WriteLine("COMPLETE STRUCTURAL PATTERNS DEMONSTRATION");
        Console.WriteLine("\nThis demo shows all three structural patterns working together!\n");

        // Use Facade to simplify everything
        var facade = new BookingSystemFacade(UniversityType.Technical);

        Console.WriteLine("STEP 1: Using FACADE to create a base booking");
        var baseBooking = facade.CreateSeminarBooking("SEMINAR-401");
        Console.WriteLine($"Created: {baseBooking}\n");

        Console.WriteLine("STEP 2: Using DECORATOR to enhance the booking");
        var decoratedBooking = BookingSystemFacade.CreateEnhancedBooking(
            baseBooking,
            includeCatering: true,
            cateringPeople: 50,
            cateringType: "Working Lunch",
            includeRecording: true,
            includeLiveStreaming: false,
            equipment: new[] { "Projector", "Whiteboard" },
            setupType: "U-Shape");

        decoratedBooking.DisplayDetails();
        Console.WriteLine($"\nEnhanced Booking Cost: ${decoratedBooking.GetCost():F2}\n");

        Console.WriteLine("STEP 3: Using COMPOSITE for multi-room event");
        var eventRooms = facade.CreateConferenceRoomGroup(
            "Tech Summit 2025",
            "MAIN-100", "BREAK-200", "BREAK-201", "WORKSHOP-301", "WORKSHOP-302");

        eventRooms.DisplayStructure();
        Console.WriteLine($"\nTotal event capacity: {eventRooms.GetTotalCapacity()} people");
        Console.WriteLine($"Number of rooms: {eventRooms.GetAllRoomCodes().Count}");

        var eventStart = DateTime.Now.AddDays(30);
        var eventEnd = DateTime.Now.AddDays(32);
        Console.WriteLine($"\nChecking availability for {eventStart:d} to {eventEnd:d}...");
        Console.WriteLine($"All rooms available: {eventRooms.IsAvailable(eventStart, eventEnd)}");

        Console.WriteLine("\n\nSTEP 4: Creating premium booking using ALL patterns via FACADE");
        var premiumEvent = facade.CreatePremiumConferenceBooking(
            "PREMIUM-HALL",
            DateTime.Now.AddDays(14),
            DateTime.Now.AddDays(14).AddHours(6),
            "Dean of Engineering",
            300);

        Console.WriteLine("\nFINAL PREMIUM EVENT DETAILS:");
        premiumEvent.DisplayDetails();
        
        Console.WriteLine($"Total Bookings Created: 3");
        Console.WriteLine($"Premium Event Cost: ${premiumEvent.GetCost():F2}");
        Console.WriteLine($"Multi-Room Capacity: {eventRooms.GetTotalCapacity()} people");

        Console.WriteLine("\n--- Final Registry State ---");
        facade.DisplayAllBookings();
    }
}
