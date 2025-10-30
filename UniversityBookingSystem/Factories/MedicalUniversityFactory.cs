using UniversityBookingSystem.Domain.Constants;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class MedicalUniversityFactory : IUniversityFactory
{
    public Room CreateLectureHall(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.LectureHall),
            Capacity = 100
        };
    }

    public Room CreateLab(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.Lab),
            Capacity = 15,
        };
    }

    public Room CreateSeminarRoom(string code)
    {
        return new Room
        {
            Code = code,
            Type = nameof(RoomType.Seminar),
            Capacity = 25,
        };
    }

    public IBookingPolicy CreatePolicy()
    {
        return new MedicalUPolicy();
    }
}