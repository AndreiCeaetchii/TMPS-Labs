namespace UniversityBookingSystem.Entities;

public class Booking
{
    public Room Room { get; set; } = new();
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public RoomLayout Layout { get; set; } = new();
    public bool Approved { get; set; }
    public string? RequestedBy { get; set; }
    
    public override string ToString()
    {
        var status = Approved ? "Approved" : "Pending";
        return $"{Room.Code} ({Room.Type}, {Room.Capacity} seats) " +
               $"booked by {RequestedBy} from {Start:g} to {End:g} [{status}] " +
               $"| Layout: {Layout.Name}";
    }
}