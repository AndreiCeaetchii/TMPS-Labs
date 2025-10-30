using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Interfaces;

public interface IBookingBuilder
{
    IBookingBuilder ForUniversity(IUniversityFactory factory);
    IBookingBuilder WithRoom(Room room);
    IBookingBuilder WithLayout(RoomLayout layout);
    IBookingBuilder On(DateTime start, DateTime end);
    IBookingBuilder RequestedBy(string personName);
    IBookingBuilder RequiresApproval(bool yes);
    Booking Build();
}