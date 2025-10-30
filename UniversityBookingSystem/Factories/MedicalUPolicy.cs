using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class MedicalUPolicy : IBookingPolicy
{
    public bool RequiresApproval(Room room, DateTime date)
    {
        // All rooms at Medical U require admin approval
        return true;
    }

    public TimeSpan MinLeadTime(Room room)
    {
        // Labs: 5 days lead time, others: 2 days
        return room.Type.Contains("Lab") ? TimeSpan.FromDays(5) : TimeSpan.FromDays(2);
    }
}