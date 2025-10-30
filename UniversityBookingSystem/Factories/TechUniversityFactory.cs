using UniversityBookingSystem.Domain.Constants;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Factories;

public class TechUniversityFactory : IUniversityFactory
{
    public Room CreateLectureHall(string code) =>
        new Room
        {
            Code = code,
            Type = nameof(RoomType.LectureHall),
            Capacity = 200,
        };

    public Room CreateLab(string code) =>
        new Room
        {
            Code = code,
            Type = nameof(RoomType.Lab),
            Capacity = 25,
        };

    public Room CreateSeminarRoom(string code) =>
        new Room
        {
            Code = code,
            Type = nameof(RoomType.Seminar),
            Capacity = 40,
        };

    public IBookingPolicy CreatePolicy() => new TechUPolicy();
}