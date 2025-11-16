using UniversityBookingSystem.Entities;

namespace UniversityBookingSystem.Composite;

/// <summary>
/// Helper class for building complex room group structures.
/// Provides a fluent interface for creating room hierarchies.
/// </summary>
public class RoomGroupBuilder
{
    private readonly RoomGroup _rootGroup;

    public RoomGroupBuilder(string groupName)
    {
        _rootGroup = new RoomGroup(groupName);
    }

    public RoomGroupBuilder AddRoom(Room room)
    {
        _rootGroup.Add(new RoomLeaf(room));
        return this;
    }

    public RoomGroupBuilder AddRoomLeaf(RoomLeaf roomLeaf)
    {
        _rootGroup.Add(roomLeaf);
        return this;
    }

    public RoomGroupBuilder AddGroup(RoomGroup group)
    {
        _rootGroup.Add(group);
        return this;
    }

    public RoomGroup Build()
    {
        return _rootGroup;
    }
}
