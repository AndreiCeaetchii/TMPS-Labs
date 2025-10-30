using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Interfaces;

public interface IUniversityFactory
{
    Room CreateLectureHall(string code);
    Room CreateLab(string code);
    Room CreateSeminarRoom(string code);
    IBookingPolicy CreatePolicy();
}