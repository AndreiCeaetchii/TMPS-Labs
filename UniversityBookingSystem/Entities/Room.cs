using UniversityBookingSystem.Domain.Constants;

namespace UniversityBookingSystem.Entities;

public class Room
{
    public string Code { get; set; } = "";
    public string Type { get; set; } = "";
    public int Capacity { get; set; }
}