using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class LiberalArtsPolicy : IBookingPolicy
{
    public bool RequiresApproval(Room room, DateTime date)
    {
        // Lecture halls need approval if booked on weekends
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
    }

    public TimeSpan MinLeadTime(Room room)
    {
        // All rooms must be booked at least 12 hours before
        return TimeSpan.FromHours(12);
    }
}