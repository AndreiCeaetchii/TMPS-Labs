using UniversityBookingSystem.Interfaces;

namespace UniversityBookingSystem.Composite;

public class RoomGroup : IRoomComponent
{
    private readonly string _groupName;
    private readonly List<IRoomComponent> _children = new();

    public RoomGroup(string groupName)
    {
        _groupName = groupName;
    }

    public void Add(IRoomComponent component)
    {
        _children.Add(component);
    }

    public void Remove(IRoomComponent component)
    {
        _children.Remove(component);
    }

    public IRoomComponent GetChild(int index)
    {
        return _children[index];
    }

    public List<IRoomComponent> GetChildren()
    {
        return new List<IRoomComponent>(_children);
    }

    public string GetCode()
    {
        return _groupName;
    }

    public int GetTotalCapacity()
    {
        return _children.Sum(child => child.GetTotalCapacity());
    }

    public void DisplayStructure(int indent = 0)
    {
        var indentation = new string(' ', indent * 2);
        Console.WriteLine($"{indentation}+ Room Group: {_groupName} (Total Capacity: {GetTotalCapacity()})");

        foreach (var child in _children)
        {
            child.DisplayStructure(indent + 1);
        }
    }

    public List<string> GetAllRoomCodes()
    {
        var codes = new List<string>();
        foreach (var child in _children)
        {
            codes.AddRange(child.GetAllRoomCodes());
        }
        return codes;
    }

    public bool IsAvailable(DateTime start, DateTime end)
    {
        // All rooms in the group must be available
        return _children.All(child => child.IsAvailable(start, end));
    }
}
