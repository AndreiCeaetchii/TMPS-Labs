using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public class SetupDecorator : BaseBookingDecorator
{
    private readonly string _setupType;
    private readonly Dictionary<string, decimal> _setupCosts = new()
    {
        { "Theater", 50.0m },
        { "U-Shape", 75.0m },
        { "Boardroom", 60.0m },
        { "Classroom", 40.0m },
        { "Banquet", 100.0m }
    };

    public SetupDecorator(IBookingComponent booking, string setupType)
        : base(booking)
    {
        _setupType = setupType;
    }

    public override string GetDescription()
    {
        return $"{base.GetDescription()} + {_setupType} Setup";
    }

    public override decimal GetCost()
    {
        var setupCost = _setupCosts.GetValueOrDefault(_setupType, 50.0m);
        return base.GetCost() + setupCost;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        var setupCost = _setupCosts.GetValueOrDefault(_setupType, 50.0m);
        Console.WriteLine($"  + {_setupType} Setup (${setupCost:F2})");
    }
}
