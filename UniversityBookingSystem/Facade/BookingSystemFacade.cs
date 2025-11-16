using UniversityBookingSystem.Composite;
using UniversityBookingSystem.Decorators;
using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Factories;
using UniversityBookingSystem.Interfaces;
using UniversityBookingSystem.Services;

namespace UniversityBookingSystem.Facade;

/// <summary>
/// Facade pattern implementation that provides a simplified interface
/// to the complex university booking system.
/// Hides the complexity of factories, builders, directors, and decorators.
/// </summary>
public class BookingSystemFacade
{
    private readonly IUniversityFactory _factory;
    private readonly BookingBuilder _builder;
    private readonly BookingDirector _director;
    private readonly BookingRegistry _registry;

    public BookingSystemFacade(UniversityType universityType)
    {
        _factory = CreateFactory(universityType);
        _builder = new BookingBuilder();
        _director = new BookingDirector();
        _registry = BookingRegistry.Instance;
    }

    private IUniversityFactory CreateFactory(UniversityType type)
    {
        return type switch
        {
            UniversityType.Technical => new TechUniversityFactory(),
            UniversityType.LiberalArts => new LiberalArtsFactory(),
            UniversityType.Medical => new MedicalUniversityFactory(),
            _ => new TechUniversityFactory()
        };
    }

    /// <summary>
    /// Simplified method to create a basic room booking.
    /// </summary>
    public Booking CreateSimpleBooking(string roomCode, RoomTypeEnum roomType, DateTime start, DateTime end, string requestedBy)
    {
        Room room = roomType switch
        {
            RoomTypeEnum.LectureHall => _factory.CreateLectureHall(roomCode),
            RoomTypeEnum.Lab => _factory.CreateLab(roomCode),
            RoomTypeEnum.Seminar => _factory.CreateSeminarRoom(roomCode),
            _ => _factory.CreateLectureHall(roomCode)
        };

        var layout = new RoomLayout
        {
            Name = $"{roomType} Standard Layout",
            Seats = room.Capacity
        };

        return _builder
            .ForUniversity(_factory)
            .WithRoom(room)
            .WithLayout(layout)
            .On(start, end)
            .RequestedBy(requestedBy)
            .Build();
    }

    /// <summary>
    /// Simplified method to create an exam booking using the director.
    /// </summary>
    public Booking CreateExamBooking(string roomCode = "A1")
    {
        return _director.CreateExamBooking(_builder, _factory, roomCode);
    }

    /// <summary>
    /// Simplified method to create a seminar booking using the director.
    /// </summary>
    public Booking CreateSeminarBooking(string roomCode = "S1")
    {
        return _director.CreateSeminarBooking(_builder, _factory, roomCode);
    }

    /// <summary>
    /// Simplified method to create a lab booking using the director.
    /// </summary>
    public Booking CreateLabBooking(string roomCode = "L1")
    {
        return _director.CreateLabBooking(_builder, _factory, roomCode);
    }

    /// <summary>
    /// Creates an enhanced booking with additional services using decorators.
    /// </summary>
    public IBookingComponent CreateEnhancedBooking(
        Booking baseBooking,
        bool includeCatering = false,
        int cateringPeople = 0,
        string cateringType = "Standard",
        bool includeRecording = false,
        bool includeLiveStreaming = false,
        string[]? equipment = null,
        string? setupType = null)
    {
        IBookingComponent component = new BaseBookingComponent(baseBooking);

        if (includeCatering && cateringPeople > 0)
        {
            component = new CateringDecorator(component, cateringType, cateringPeople);
        }

        if (includeRecording || includeLiveStreaming)
        {
            component = new RecordingDecorator(component, includeRecording, includeLiveStreaming);
        }

        if (equipment != null && equipment.Length > 0)
        {
            component = new EquipmentDecorator(component, equipment);
        }

        if (!string.IsNullOrEmpty(setupType))
        {
            component = new SetupDecorator(component, setupType);
        }

        return component;
    }

    /// <summary>
    /// Creates a room group for large events spanning multiple rooms.
    /// </summary>
    public RoomGroup CreateConferenceRoomGroup(string groupName, params string[] roomCodes)
    {
        var group = new RoomGroup(groupName);

        foreach (var code in roomCodes)
        {
            // Alternate between different room types for variety
            var roomIndex = Array.IndexOf(roomCodes, code);
            var roomTypeIndex = roomIndex % 3;
            var room = roomTypeIndex switch
            {
                0 => _factory.CreateLectureHall(code),
                1 => _factory.CreateSeminarRoom(code),
                _ => _factory.CreateLab(code)
            };

            group.Add(new RoomLeaf(room));
        }

        return group;
    }

    /// <summary>
    /// Checks if a room or room group is available for booking.
    /// </summary>
    public bool CheckAvailability(IRoomComponent roomComponent, DateTime start, DateTime end)
    {
        return roomComponent.IsAvailable(start, end);
    }

    /// <summary>
    /// Gets all current bookings from the registry.
    /// </summary>
    public IReadOnlyList<Booking> GetAllBookings()
    {
        return _registry.AllBookings;
    }

    /// <summary>
    /// Displays all bookings in the system.
    /// </summary>
    public void DisplayAllBookings()
    {
        _registry.PrintAll();
    }

    /// <summary>
    /// One-stop method to create a complete premium booking with all features.
    /// This demonstrates the facade hiding ALL complexity.
    /// </summary>
    public IBookingComponent CreatePremiumConferenceBooking(
        string roomCode,
        DateTime start,
        DateTime end,
        string organizer,
        int attendees)
    {
        // Step 1: Create base booking
        var baseBooking = CreateSimpleBooking(
            roomCode,
            RoomTypeEnum.LectureHall,
            start,
            end,
            organizer);

        // Step 2: Enhance with all premium features
        return CreateEnhancedBooking(
            baseBooking,
            includeCatering: true,
            cateringPeople: attendees,
            cateringType: "Premium Catering",
            includeRecording: true,
            includeLiveStreaming: true,
            equipment: new[] { "Laser Projector", "Sound System", "Video Conferencing" },
            setupType: "Theater");
    }
}

public enum UniversityType
{
    Technical,
    LiberalArts,
    Medical
}

public enum RoomTypeEnum
{
    LectureHall,
    Lab,
    Seminar
}
