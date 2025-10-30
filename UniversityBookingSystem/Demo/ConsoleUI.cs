using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Factories;
using UniversityBookingSystem.Interfaces;
using UniversityBookingSystem.Services;

namespace UniversityBookingSystem.Demo;

public abstract class ConsoleUI
{
    public static void Run()
    {
        Console.WriteLine("=== UniRoomBooker Demo (with Director) ===\n");

        Console.WriteLine("Choose University:");
        Console.WriteLine("1. Tech University");
        Console.WriteLine("2. Liberal Arts University");
        Console.WriteLine("3. Medical University");
        Console.Write("> ");
        var option = Console.ReadLine();

        IUniversityFactory factory = option switch
        {
            "1" => new TechUniversityFactory(),
            "2" => new LiberalArtsFactory(),
            "3" => new MedicalUniversityFactory(),
            _ => new TechUniversityFactory()
        };
        
        var builder = new BookingBuilder();
        var director = new BookingDirector();

        Console.WriteLine("Available booking templates:");
        Console.WriteLine("1. Exam Booking");
        Console.WriteLine("2. Seminar Booking");
        Console.WriteLine("3. Lab Session");
        Console.Write("> ");
        var templateChoice = Console.ReadLine();

        Booking booking;

        switch (templateChoice)
        {
            case "1":
                booking = director.CreateExamBooking(builder, factory, "A1");
                break;

            case "2":
                booking = director.CreateSeminarBooking(builder, factory, "S1");
                break;

            case "3":
                booking = director.CreateLabBooking(builder, factory, "L1");
                break;

            default:
                booking = director.CreateExamBooking(builder, factory, "A1");
                break;
        }

        Console.WriteLine("\nBooking Created Successfully!");
        Console.WriteLine(booking);

        var registry = BookingRegistry.Instance;
        Console.WriteLine("\n--- Singleton Registry Content ---");
        registry.PrintAll();

        Console.WriteLine("\nTry to create another booking for the same room/time to test conflict check...");
        try
        {
            booking = director.CreateExamBooking(builder, factory, "A1");
            Console.WriteLine("\nBooking Created Successfully!");
            Console.WriteLine(booking);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Conflict detected: {ex.Message}");
        }

        Console.WriteLine("\nDemo finished. Press any key to exit...");
        Console.ReadKey();
    }
}