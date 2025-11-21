using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Decorators;

public class EquipmentDecorator : BaseBookingDecorator
{
    private readonly List<string> _equipment;
    private const decimal CostPerEquipment = 25.0m;

    public EquipmentDecorator(IBookingComponent booking, params string[] equipment)
        : base(booking)
    {
        _equipment = new List<string>(equipment);
    }

    public override string GetDescription()
    {
        var equipmentList = string.Join(", ", _equipment);
        return $"{base.GetDescription()} + Equipment ({equipmentList})";
    }

    public override decimal GetCost()
    {
        return base.GetCost() + (_equipment.Count * CostPerEquipment);
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        Console.WriteLine($"  + Equipment: {string.Join(", ", _equipment)} (${_equipment.Count * CostPerEquipment:F2})");
    }
}
