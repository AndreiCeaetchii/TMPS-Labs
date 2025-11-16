namespace UniversityBookingSystem.Composite;

/// <summary>
/// Component interface for the Composite pattern.
/// Defines common operations for both individual rooms and room groups.
/// </summary>
public interface IRoomComponent
{
    string GetCode();
    int GetTotalCapacity();
    void DisplayStructure(int indent = 0);
    List<string> GetAllRoomCodes();
    bool IsAvailable(DateTime start, DateTime end);
}
