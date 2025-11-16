namespace UniversityBookingSystem.Decorators;

/// <summary>
/// Base component interface for the Decorator pattern.
/// Defines the contract for all booking components and decorators.
/// </summary>
public interface IBookingComponent
{
    string GetDescription();
    decimal GetCost();
    void DisplayDetails();
}
