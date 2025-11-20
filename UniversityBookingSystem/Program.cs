using UniversityBookingSystem.Demo;

Console.Title = "UniRoomBooker Demo";

Console.WriteLine("Select which demo to run:");
Console.WriteLine("1. Creational Patterns Demo (Factory, Builder, Singleton, Director)");
Console.WriteLine("2. Structural Patterns Demo (Decorator, Composite, Facade)");
Console.Write("\n> ");

var choice = Console.ReadLine();

Console.WriteLine();

if (choice == "2")
{
    StructuralPatternsDemo.Run();
}
else
{
    ConsoleUI.Run();
}