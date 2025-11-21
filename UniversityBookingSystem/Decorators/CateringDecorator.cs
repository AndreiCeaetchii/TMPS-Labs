using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public class CateringDecorator : BaseBookingDecorator
{
    private readonly string _cateringType;
    private readonly int _numberOfPeople;
    private const decimal CostPerPerson = 15.0m;

    public CateringDecorator(IBookingComponent booking, string cateringType, int numberOfPeople)
        : base(booking)
    {
        _cateringType = cateringType;
        _numberOfPeople = numberOfPeople;
    }

    public override string GetDescription()
    {
        return $"{base.GetDescription()} + Catering ({_cateringType} for {_numberOfPeople} people)";
    }

    public override decimal GetCost()
    {
        return base.GetCost() + (_numberOfPeople * CostPerPerson);
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"  + Catering: {_cateringType} for {_numberOfPeople} people (${_numberOfPeople * CostPerPerson:F2})");
    }
}
