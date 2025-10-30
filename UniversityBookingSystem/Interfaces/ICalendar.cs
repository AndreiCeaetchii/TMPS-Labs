using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Interfaces;

public interface ICalendar
{
    bool IsAvailable(Room room, DateTime start, DateTime end);
}