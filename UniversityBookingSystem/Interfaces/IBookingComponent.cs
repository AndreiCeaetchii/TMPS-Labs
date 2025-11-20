namespace UniversityBookingSystem.Interfaces;

public interface IBookingComponent
{
    string GetDescription();
    decimal GetCost();
    void DisplayDetails();
}
