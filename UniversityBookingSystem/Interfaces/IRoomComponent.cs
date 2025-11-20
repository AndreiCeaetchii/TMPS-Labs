namespace UniversityBookingSystem.Interfaces;

public interface IRoomComponent
{
    string GetCode();
    int GetTotalCapacity();
    void DisplayStructure(int indent = 0);
    List<string> GetAllRoomCodes();
    bool IsAvailable(DateTime start, DateTime end);
}
