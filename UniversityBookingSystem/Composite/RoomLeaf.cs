using UniversityBookingSystem.Entities;
using UniversityBookingSystem.Services;

namespace UniversityBookingSystem.Composite;

/// <summary>
/// Leaf component representing a single room in the composite structure.
/// Cannot contain other room components.
/// </summary>
public class RoomLeaf : IRoomComponent
{
    private readonly Room _room;

    public RoomLeaf(Room room)
    {
        _room = room;
    }

    public Room Room => _room;

    public string GetCode()
    {
        return _room.Code;
    }

    public int GetTotalCapacity()
    {
        return _room.Capacity;
    }

    public void DisplayStructure(int indent = 0)
    {
        var indentation = new string(' ', indent * 2);
        Console.WriteLine($"{indentation}- Room {_room.Code} ({_room.Type}, Capacity: {_room.Capacity})");
    }

    public List<string> GetAllRoomCodes()
    {
        return new List<string> { _room.Code };
    }

    public bool IsAvailable(DateTime start, DateTime end)
    {
        var registry = BookingRegistry.Instance;
        return !registry.HasConflict(_room, start, end);
    }
}
