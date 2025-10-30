using UniversityBookingSystem.Domain.Constants;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class LiberalArtsFactory : IUniversityFactory
{
    public Room CreateLectureHall(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.LectureHall),
            Capacity = 120
        };
    }

    public Room CreateLab(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.Lab),
            Capacity = 20,
        };
    }

    public Room CreateSeminarRoom(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.Seminar),
            Capacity = 30,
        };
    }

    public IBookingPolicy CreatePolicy()
    {
        return new LiberalArtsPolicy();    }
}