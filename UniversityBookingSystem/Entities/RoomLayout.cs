namespace UniversityBookingSystem.Entities;

public class RoomLayout
{
    public string Name { get; set; } = "";
    public int Seats { get; set; }
    public List<string> Equipment { get; } = new();
}