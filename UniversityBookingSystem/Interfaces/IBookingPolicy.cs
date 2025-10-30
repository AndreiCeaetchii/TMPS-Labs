using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Interfaces;

public interface IBookingPolicy
{
    bool RequiresApproval(Room room, DateTime date);
    TimeSpan MinLeadTime(Room room);
}