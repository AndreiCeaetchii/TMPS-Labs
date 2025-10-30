using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Services;

public class BookingDirector
{
    public Booking CreateExamBooking(IBookingBuilder builder, IUniversityFactory factory, string roomCode)
    {
        var layout = new RoomLayout
        {
            Name = "Exam Layout",
            Seats = 100,
            Equipment = { "Projector", "Clock" }
        };

        var room = factory.CreateLectureHall(roomCode);

        return builder
            .ForUniversity(factory)
            .WithRoom(room)
            .WithLayout(layout)
            .On(DateTime.Now.AddHours(1), DateTime.Now.AddHours(3))
            .RequestedBy("Exam Committee")
            .RequiresApproval(false)
            .Build();
    }

    public Booking CreateSeminarBooking(IBookingBuilder builder, IUniversityFactory factory, string roomCode)
    {
        var layout = new RoomLayout
        {
            Name = "Seminar U-Shape",
            Seats = 40,
            Equipment = { "Whiteboard", "Microphone" }
        };

        var room = factory.CreateSeminarRoom(roomCode);

        return builder
            .ForUniversity(factory)
            .WithRoom(room)
            .WithLayout(layout)
            .On(DateTime.Now.AddHours(2), DateTime.Now.AddHours(4))
            .RequestedBy("Professor Adams")
            .RequiresApproval(false)
            .Build();
    }

    public Booking CreateLabBooking(IBookingBuilder builder, IUniversityFactory factory, string roomCode)
    {
        var layout = new RoomLayout
        {
            Name = "Computer Lab",
            Seats = 20,
            Equipment = { "PCs", "Projector" }
        };

        var room = factory.CreateLab(roomCode);

        return builder
            .ForUniversity(factory)
            .WithRoom(room)
            .WithLayout(layout)
            .On(DateTime.Now.AddHours(3), DateTime.Now.AddHours(5))
            .RequestedBy("Lab Supervisor")
            .RequiresApproval(true)
            .Build();
    }
}