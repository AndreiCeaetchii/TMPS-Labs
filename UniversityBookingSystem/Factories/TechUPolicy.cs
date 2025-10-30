using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class TechUPolicy : IBookingPolicy
{
    public bool RequiresApproval(Room room, DateTime date)
    {
        // Tech labs always require approval
        return room.Type.Contains("Lab");
    }

    public TimeSpan MinLeadTime(Room room)
    {
        // Labs need 3 days lead time; others 1 day
        return room.Type.Contains("Lab") ? TimeSpan.FromDays(3) : TimeSpan.FromDays(1);
    }
}