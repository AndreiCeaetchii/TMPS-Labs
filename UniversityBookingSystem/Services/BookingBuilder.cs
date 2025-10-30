using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Services;

public class BookingBuilder : IBookingBuilder
{
    private IUniversityFactory? _factory;
    private Room? _room;
    private RoomLayout? _layout;
    private DateTime _start;
    private DateTime _end;
    private string _requestedBy = "Unknown";
    private bool _forceApproval;

    public IBookingBuilder ForUniversity(IUniversityFactory factory)
    {
        _factory = factory;
        return this;
    }

    public IBookingBuilder WithRoom(Room room)
    {
        _room = room;
        return this;
    }

    public IBookingBuilder WithLayout(RoomLayout layout)
    {
        _layout = layout;
        return this;
    }

    public IBookingBuilder On(DateTime start, DateTime end)
    {
        _start = start;
        _end = end;
        return this;
    }

    public IBookingBuilder RequestedBy(string personName)
    {
        _requestedBy = personName;
        return this;
    }

    public IBookingBuilder RequiresApproval(bool yes)
    {
        _forceApproval = yes;
        return this;
    }

    public Booking Build()
    {
        if (_factory is null || _room is null || _layout is null)
            throw new InvalidOperationException("BookingBuilder: missing required fields.");

        var registry = BookingRegistry.Instance;
        if (registry.HasConflict(_room, _start, _end))
            throw new InvalidOperationException("This room is already booked for the selected time.");

        var policy = _factory.CreatePolicy();
        var needsApproval = _forceApproval || policy.RequiresApproval(_room, _start);

        var booking = new Booking
        {
            Room = _room,
            Layout = _layout,
            Start = _start,
            End = _end,
            RequestedBy = _requestedBy,
            Approved = !needsApproval
        };

        registry.Add(booking);
        return booking;
    }
}