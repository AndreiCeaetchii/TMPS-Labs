using UniversityBookingSystem.Demo;

Console.Title = "UniRoomBooker Demo";

Console.WriteLine("Select which demo to run:");
Console.WriteLine("1. Creational Patterns Demo (Factory, Builder, Singleton, Director)");
Console.WriteLine("2. Structural Patterns Demo (Decorator, Composite, Facade)");
Console.WriteLine("3. Behavioral Patterns Demo (Observer, State, Command)");
Console.Write("\n> ");

var choice = Console.ReadLine();

Console.WriteLine();

switch (choice)
{
    case "2":
        StructuralPatternsDemo.Run();
        break;
    case "3":
        BehavioralPatternsDemo.Run();
        break;
    default:
        ConsoleUI.Run();
        break;
}