# University Booking System - Structural Design Patterns Implementation

## Table of Contents
- [Overview](#overview)
- [Structural Design Patterns Implemented](#structural-design-patterns-implemented)
  - [1. Decorator Pattern](#1-decorator-pattern)
  - [2. Composite Pattern](#2-composite-pattern)
  - [3. Facade Pattern](#3-facade-pattern)
- [Architecture](#architecture)
- [How to Run](#how-to-run)
- [Pattern Interactions](#pattern-interactions)
- [Code Examples](#code-examples)
- [Benefits](#benefits)

---

## Overview

This project demonstrates the implementation of **three structural design patterns** in a university room booking system. Structural design patterns focus on how classes and objects are composed to form larger structures while keeping these structures flexible and efficient.

The three patterns implemented are:
1. **Decorator Pattern** - Dynamically adds responsibilities to objects
2. **Composite Pattern** - Composes objects into tree structures to represent part-whole hierarchies
3. **Facade Pattern** - Provides a simplified interface to a complex subsystem

These patterns build upon the previously implemented creational patterns (Abstract Factory, Builder, Singleton, Director) to create a comprehensive, maintainable, and extensible booking system.

---

## Structural Design Patterns Implemented

### 1. Decorator Pattern

#### Purpose
The Decorator pattern allows adding new functionality to objects dynamically without altering their structure. It provides a flexible alternative to subclassing for extending functionality.

#### Implementation

**Component Interface:**
```csharp
public interface IBookingComponent
{
    string GetDescription();
    decimal GetCost();
    void DisplayDetails();
}
```

**Base Component:**
```csharp
public class BaseBookingComponent : IBookingComponent
{
    private readonly Booking _booking;
    private const decimal BaseRoomCost = 50.0m;

    public BaseBookingComponent(Booking booking)
    {
        _booking = booking;
    }

    public virtual string GetDescription()
    {
        return $"Room {_booking.Room.Code} ({_booking.Room.Type})";
    }

    public virtual decimal GetCost()
    {
        var hours = (_booking.End - _booking.Start).TotalHours;
        return BaseRoomCost * (decimal)hours;
    }

    public virtual void DisplayDetails()
    {
        Console.WriteLine($"Booking: {_booking}");
        Console.WriteLine($"Description: {GetDescription()}");
        Console.WriteLine($"Base Cost: ${GetCost():F2}");
    }
}
```

**Abstract Decorator:**
```csharp
public abstract class BookingDecorator : IBookingComponent
{
    protected readonly IBookingComponent _wrappedBooking;

    protected BookingDecorator(IBookingComponent booking)
    {
        _wrappedBooking = booking;
    }

    public virtual string GetDescription() => _wrappedBooking.GetDescription();
    public virtual decimal GetCost() => _wrappedBooking.GetCost();
    public virtual void DisplayDetails() => _wrappedBooking.DisplayDetails();
}
```

#### Concrete Decorators

**1. CateringDecorator** - Adds catering services
```csharp
public class CateringDecorator : BookingDecorator
{
    private readonly string _cateringType;
    private readonly int _numberOfPeople;
    private const decimal CostPerPerson = 15.0m;

    public CateringDecorator(IBookingComponent booking, string cateringType, int numberOfPeople)
        : base(booking)
    {
        _cateringType = cateringType;
        _numberOfPeople = numberOfPeople;
    }

    public override string GetDescription()
    {
        return $"{base.GetDescription()} + Catering ({_cateringType} for {_numberOfPeople} people)";
    }

    public override decimal GetCost()
    {
        return base.GetCost() + (_numberOfPeople * CostPerPerson);
    }
}
```

**2. EquipmentDecorator** - Adds special equipment
```csharp
public class EquipmentDecorator : BookingDecorator
{
    private readonly List<string> _equipment;
    private const decimal CostPerEquipment = 25.0m;

    public override decimal GetCost()
    {
        return base.GetCost() + (_equipment.Count * CostPerEquipment);
    }
}
```

**3. RecordingDecorator** - Adds recording and streaming services
```csharp
public class RecordingDecorator : BookingDecorator
{
    private readonly bool _liveStreaming;
    private readonly bool _recording;
    private const decimal RecordingCost = 75.0m;
    private const decimal StreamingCost = 100.0m;

    public override decimal GetCost()
    {
        var additionalCost = 0m;
        if (_recording) additionalCost += RecordingCost;
        if (_liveStreaming) additionalCost += StreamingCost;
        return base.GetCost() + additionalCost;
    }
}
```

**4. SetupDecorator** - Adds special room setup
```csharp
public class SetupDecorator : BookingDecorator
{
    private readonly string _setupType;
    private readonly Dictionary<string, decimal> _setupCosts = new()
    {
        { "Theater", 50.0m },
        { "U-Shape", 75.0m },
        { "Boardroom", 60.0m },
        { "Classroom", 40.0m },
        { "Banquet", 100.0m }
    };
}
```

#### Usage Example
```csharp
// Start with base booking
IBookingComponent booking = new BaseBookingComponent(baseBooking);

// Dynamically add features by wrapping with decorators
booking = new CateringDecorator(booking, "Premium Lunch", 100);
booking = new EquipmentDecorator(booking, "Projector", "Microphone", "Whiteboard");
booking = new RecordingDecorator(booking, recording: true, liveStreaming: true);
booking = new SetupDecorator(booking, "Theater");

// Display final details with all enhancements
booking.DisplayDetails();
Console.WriteLine($"Total Cost: ${booking.GetCost():F2}");
```

#### Benefits
- **Open/Closed Principle**: Add new decorators without modifying existing code
- **Single Responsibility**: Each decorator handles one specific enhancement
- **Flexible Composition**: Mix and match decorators as needed
- **Runtime Configuration**: Add/remove features dynamically
- **Cost Transparency**: Each decorator adds its cost independently

#### File Location
- `Decorators/IBookingComponent.cs` - Component interface
- `Decorators/BaseBookingComponent.cs` - Concrete component
- `Decorators/BookingDecorator.cs` - Base decorator
- `Decorators/CateringDecorator.cs` - Catering decorator
- `Decorators/EquipmentDecorator.cs` - Equipment decorator
- `Decorators/RecordingDecorator.cs` - Recording decorator
- `Decorators/SetupDecorator.cs` - Setup decorator

---

### 2. Composite Pattern

#### Purpose
The Composite pattern composes objects into tree structures to represent part-whole hierarchies. It lets clients treat individual objects and compositions of objects uniformly.

#### Implementation

**Component Interface:**
```csharp
public interface IRoomComponent
{
    string GetCode();
    int GetTotalCapacity();
    void DisplayStructure(int indent = 0);
    List<string> GetAllRoomCodes();
    bool IsAvailable(DateTime start, DateTime end);
}
```

**Leaf Component (Individual Room):**
```csharp
public class RoomLeaf : IRoomComponent
{
    private readonly Room _room;

    public RoomLeaf(Room room)
    {
        _room = room;
    }

    public string GetCode() => _room.Code;

    public int GetTotalCapacity() => _room.Capacity;

    public void DisplayStructure(int indent = 0)
    {
        var indentation = new string(' ', indent * 2);
        Console.WriteLine($"{indentation}- Room {_room.Code} ({_room.Type}, Capacity: {_room.Capacity})");
    }

    public List<string> GetAllRoomCodes() => new List<string> { _room.Code };

    public bool IsAvailable(DateTime start, DateTime end)
    {
        var registry = BookingRegistry.Instance;
        return !registry.HasConflict(_room, start, end);
    }
}
```

**Composite Component (Room Group):**
```csharp
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

    public string GetCode() => _groupName;

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
```

#### Usage Example
```csharp
var factory = new TechUniversityFactory();

// Create individual rooms (leaves)
var mainHall = new RoomLeaf(factory.CreateLectureHall("MH-100"));
var lab1 = new RoomLeaf(factory.CreateLab("LAB-201"));
var lab2 = new RoomLeaf(factory.CreateLab("LAB-202"));

// Create a composite (room group)
var labWing = new RoomGroup("Computer Science Lab Wing");
labWing.Add(lab1);
labWing.Add(lab2);

// Create a larger composite containing other composites and leaves
var conferenceCenter = new RoomGroup("University Conference Center");
conferenceCenter.Add(mainHall);
conferenceCenter.Add(labWing);

// Treat individual rooms and groups uniformly
conferenceCenter.DisplayStructure();
Console.WriteLine($"Total Capacity: {conferenceCenter.GetTotalCapacity()}");

// Check availability for entire group
bool available = conferenceCenter.IsAvailable(startTime, endTime);
```

#### Benefits
- **Uniform Treatment**: Work with individual rooms and groups through same interface
- **Tree Structure**: Natural representation of part-whole hierarchies
- **Recursive Operations**: Operations propagate through tree structure automatically
- **Easy Extension**: Add new types of components without changing existing code
- **Aggregate Information**: Calculate totals (capacity, availability) across complex structures

#### File Location
- `Composite/IRoomComponent.cs` - Component interface
- `Composite/RoomLeaf.cs` - Leaf (individual room)
- `Composite/RoomGroup.cs` - Composite (room group)
- `Composite/RoomGroupBuilder.cs` - Helper builder for composites

---

### 3. Facade Pattern

#### Purpose
The Facade pattern provides a simplified, unified interface to a complex subsystem. It makes the subsystem easier to use by hiding its complexity behind a single, high-level interface.

#### Implementation

```csharp
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

    // Simplified method hiding Factory + Builder complexity
    public Booking CreateSimpleBooking(string roomCode, RoomTypeEnum roomType,
        DateTime start, DateTime end, string requestedBy)
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

    // Simplified method hiding Director complexity
    public Booking CreateExamBooking(string roomCode = "A1")
    {
        return _director.CreateExamBooking(_builder, _factory, roomCode);
    }

    // Simplified method hiding Decorator complexity
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
            component = new CateringDecorator(component, cateringType, cateringPeople);

        if (includeRecording || includeLiveStreaming)
            component = new RecordingDecorator(component, includeRecording, includeLiveStreaming);

        if (equipment != null && equipment.Length > 0)
            component = new EquipmentDecorator(component, equipment);

        if (!string.IsNullOrEmpty(setupType))
            component = new SetupDecorator(component, setupType);

        return component;
    }

    // Simplified method hiding Composite complexity
    public RoomGroup CreateConferenceRoomGroup(string groupName, params string[] roomCodes)
    {
        var group = new RoomGroup(groupName);

        foreach (var code in roomCodes)
        {
            var roomIndex = Array.IndexOf(roomCodes, code);
            var room = roomIndex % 3 switch
            {
                0 => _factory.CreateLectureHall(code),
                1 => _factory.CreateSeminarRoom(code),
                _ => _factory.CreateLab(code)
            };

            group.Add(new RoomLeaf(room));
        }

        return group;
    }

    // ONE METHOD that combines ALL patterns!
    public IBookingComponent CreatePremiumConferenceBooking(
        string roomCode, DateTime start, DateTime end, string organizer, int attendees)
    {
        // Create base booking (uses Factory + Builder)
        var baseBooking = CreateSimpleBooking(roomCode, RoomTypeEnum.LectureHall,
            start, end, organizer);

        // Enhance with all features (uses Decorators)
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
```

#### Subsystems Hidden by Facade
The facade hides the complexity of:
1. **Abstract Factory Pattern** - Room and policy creation
2. **Builder Pattern** - Step-by-step booking construction
3. **Director Pattern** - Template-based booking creation
4. **Singleton Pattern** - Registry management
5. **Decorator Pattern** - Dynamic feature enhancement
6. **Composite Pattern** - Room grouping

#### Usage Example

**Without Facade (Complex):**
```csharp
// Client must know about all subsystems
var factory = new TechUniversityFactory();
var builder = new BookingBuilder();
var room = factory.CreateLectureHall("A101");
var layout = new RoomLayout { Name = "Standard", Seats = 100 };

var booking = builder
    .ForUniversity(factory)
    .WithRoom(room)
    .WithLayout(layout)
    .On(DateTime.Now, DateTime.Now.AddHours(2))
    .RequestedBy("User")
    .Build();

IBookingComponent component = new BaseBookingComponent(booking);
component = new CateringDecorator(component, "Lunch", 50);
component = new EquipmentDecorator(component, "Projector");
```

**With Facade (Simple):**
```csharp
// Client uses simple facade interface
var facade = new BookingSystemFacade(UniversityType.Technical);

// One simple method call!
var premiumBooking = facade.CreatePremiumConferenceBooking(
    "A101",
    DateTime.Now,
    DateTime.Now.AddHours(2),
    "Conference Organizer",
    50);
```

#### Benefits
- **Simplified Interface**: Single point of entry for complex operations
- **Reduced Coupling**: Clients don't depend on internal subsystem classes
- **Easier to Use**: Hide complexity from users who don't need it
- **Flexibility**: Advanced users can still access subsystems directly
- **Better Maintainability**: Changes to subsystems don't affect facade clients

#### File Location
- `Facade/BookingSystemFacade.cs` - Main facade implementation

---

## Architecture

### System Structure

```
UniversityBookingSystem/
├── Entities/               # Domain entities
│   ├── Booking.cs
│   ├── Room.cs
│   └── RoomLayout.cs
│
├── Interfaces/             # Contracts and abstractions
│   ├── IUniversityFactory.cs
│   ├── IBookingBuilder.cs
│   └── IBookingPolicy.cs
│
├── Factories/              # Abstract Factory pattern
│   ├── TechUniversityFactory.cs
│   ├── LiberalArtsFactory.cs
│   ├── MedicalUniversityFactory.cs
│   └── [Policy implementations]
│
├── Services/               # Builder, Director, Singleton
│   ├── BookingBuilder.cs
│   ├── BookingDirector.cs
│   └── BookingRegistry.cs (Singleton)
│
├── Decorators/             # Decorator pattern ⭐
│   ├── IBookingComponent.cs
│   ├── BaseBookingComponent.cs
│   ├── BookingDecorator.cs
│   ├── CateringDecorator.cs
│   ├── EquipmentDecorator.cs
│   ├── RecordingDecorator.cs
│   └── SetupDecorator.cs
│
├── Composite/              # Composite pattern ⭐
│   ├── IRoomComponent.cs
│   ├── RoomLeaf.cs
│   ├── RoomGroup.cs
│   └── RoomGroupBuilder.cs
│
├── Facade/                 # Facade pattern ⭐
│   └── BookingSystemFacade.cs
│
├── Demo/                   # Demonstrations
│   ├── ConsoleUI.cs (Creational patterns demo)
│   └── StructuralPatternsDemo.cs (Structural patterns demo)
│
├── Program.cs              # Entry point
├── report.md              # Creational patterns report
└── README.md              # This file
```

---

## How to Run

### Prerequisites
- .NET 8.0 SDK or later
- C# IDE (Visual Studio, Rider, or VS Code)

### Running the Application

1. **Navigate to project directory:**
   ```bash
   cd UniversityBookingSystem
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Select demo type:**
   - Option 1: Creational Patterns Demo
   - Option 2: Structural Patterns Demo

5. **For Structural Patterns Demo, choose:**
   - Option 1: Decorator Pattern Demo
   - Option 2: Composite Pattern Demo
   - Option 3: Facade Pattern Demo
   - Option 4: Complete Demo (all patterns together)

---

## Pattern Interactions

### How Patterns Work Together

```
┌─────────────────────────────────────────────────────────┐
│                    FACADE LAYER                         │
│    (BookingSystemFacade - Simplified Interface)         │
└─────────┬───────────────────────────────────────────────┘
          │
          ├──> Abstract Factory (Creates Rooms & Policies)
          │
          ├──> Builder (Constructs Bookings)
          │
          ├──> Director (Template Bookings)
          │
          ├──> Decorator (Enhances Bookings)
          │        │
          │        ├──> CateringDecorator
          │        ├──> EquipmentDecorator
          │        ├──> RecordingDecorator
          │        └──> SetupDecorator
          │
          ├──> Composite (Room Groups)
          │        │
          │        ├──> RoomLeaf (Individual Rooms)
          │        └──> RoomGroup (Composite)
          │
          └──> Singleton (BookingRegistry)
```

### Example Flow

1. **Client** calls facade method: `CreatePremiumConferenceBooking()`
2. **Facade** uses **Factory** to create room
3. **Facade** uses **Builder** to construct base booking
4. **Facade** wraps booking with **Decorators** for enhancements
5. **Decorators** chain together to add features and calculate costs
6. **Builder** uses **Singleton** registry to check conflicts
7. Final enhanced booking returned to client

---

## Code Examples

### Example 1: Decorator Pattern

```csharp
// Create base booking
var factory = new TechUniversityFactory();
var booking = new Booking
{
    Room = factory.CreateLectureHall("A101"),
    Start = DateTime.Now.AddHours(2),
    End = DateTime.Now.AddHours(4),
    RequestedBy = "Dr. Smith"
};

// Start with base component
IBookingComponent enhanced = new BaseBookingComponent(booking);

// Add catering
enhanced = new CateringDecorator(enhanced, "Premium Lunch", 100);

// Add equipment
enhanced = new EquipmentDecorator(enhanced, "Projector", "Microphone");

// Add recording
enhanced = new RecordingDecorator(enhanced, true, true);

// Display all details and total cost
enhanced.DisplayDetails();
Console.WriteLine($"Total: ${enhanced.GetCost():F2}");
```

### Example 2: Composite Pattern

```csharp
var factory = new TechUniversityFactory();

// Create individual rooms
var hall1 = new RoomLeaf(factory.CreateLectureHall("H1"));
var lab1 = new RoomLeaf(factory.CreateLab("L1"));
var lab2 = new RoomLeaf(factory.CreateLab("L2"));

// Create lab group
var labWing = new RoomGroup("Engineering Labs");
labWing.Add(lab1);
labWing.Add(lab2);

// Create complete conference center
var center = new RoomGroup("Conference Center");
center.Add(hall1);
center.Add(labWing); // Add composite to composite!

// Work with entire structure uniformly
center.DisplayStructure();
Console.WriteLine($"Total Capacity: {center.GetTotalCapacity()}");

// Check if all rooms available
bool canBook = center.IsAvailable(startTime, endTime);
```

### Example 3: Facade Pattern

```csharp
// Simple facade interface hides all complexity
var facade = new BookingSystemFacade(UniversityType.Technical);

// Create simple booking (hides Factory + Builder)
var booking = facade.CreateSimpleBooking(
    "A101",
    RoomTypeEnum.LectureHall,
    DateTime.Now.AddHours(2),
    DateTime.Now.AddHours(4),
    "Prof. Johnson");

// Create enhanced booking (hides Decorators)
var enhanced = facade.CreateEnhancedBooking(
    booking,
    includeCatering: true,
    cateringPeople: 50,
    includeRecording: true,
    equipment: new[] { "Projector", "Microphone" });

// Create room group (hides Composite)
var rooms = facade.CreateConferenceRoomGroup(
    "Tech Summit",
    "A1", "A2", "L1", "L2");

// Premium booking - ONE call does everything!
var premium = facade.CreatePremiumConferenceBooking(
    "PREMIUM-HALL",
    DateTime.Now.AddDays(7),
    DateTime.Now.AddDays(7).AddHours(8),
    "Conference Committee",
    200);

premium.DisplayDetails();
```

### Example 4: All Patterns Together

```csharp
// Use facade to simplify everything
var facade = new BookingSystemFacade(UniversityType.Technical);

// Step 1: Create base booking (Factory + Builder)
var booking = facade.CreateSeminarBooking("SEM-401");

// Step 2: Enhance with decorators
var decorated = facade.CreateEnhancedBooking(
    booking,
    includeCatering: true,
    cateringPeople: 50,
    includeRecording: true);

// Step 3: Create multi-room composite
var rooms = facade.CreateConferenceRoomGroup(
    "Annual Conference",
    "MAIN", "BREAK-1", "BREAK-2", "WORKSHOP-1");

rooms.DisplayStructure();
decorated.DisplayDetails();

// All managed through simple facade interface!
```

---

## Benefits

### Decorator Pattern Benefits
✅ **Flexible Extension**: Add features without modifying existing code
✅ **Single Responsibility**: Each decorator has one job
✅ **Runtime Composition**: Mix and match features dynamically
✅ **Open/Closed Principle**: Open for extension, closed for modification
✅ **Transparent Wrapping**: Decorators implement same interface as components

### Composite Pattern Benefits
✅ **Uniform Treatment**: Work with individual and composite objects uniformly
✅ **Tree Structures**: Natural hierarchy representation
✅ **Recursive Composition**: Infinite nesting levels supported
✅ **Simplified Client Code**: No need to distinguish between leaf and composite
✅ **Easy Extension**: Add new component types without changing existing code

### Facade Pattern Benefits
✅ **Simplified Interface**: One simple entry point for complex operations
✅ **Reduced Coupling**: Clients don't depend on subsystem internals
✅ **Layered Architecture**: Clear separation between interface and implementation
✅ **Easier Learning Curve**: New users can start with facade
✅ **Flexibility**: Advanced users can still access subsystems directly

### Combined Benefits
🎯 **Maintainability**: Clear separation of concerns
🎯 **Scalability**: Easy to add new features and components
🎯 **Testability**: Each pattern can be tested independently
🎯 **Reusability**: Components can be reused in different contexts
🎯 **Professional Design**: Industry-standard architectural patterns

---

## Conclusion

This University Booking System demonstrates how structural design patterns can work together to create a flexible, maintainable, and user-friendly application:

- **Decorator Pattern** provides runtime customization of bookings with additional services
- **Composite Pattern** enables hierarchical room management for complex events
- **Facade Pattern** simplifies the entire system with a clean, intuitive interface

These structural patterns complement the previously implemented creational patterns (Abstract Factory, Builder, Singleton, Director), creating a comprehensive system that demonstrates professional software architecture principles.

The implementation showcases:
- Object composition over inheritance
- Flexible and extensible design
- Clean separation of concerns
- Easy-to-use interfaces
- Professional coding standards

---

## Author

Implementation for TMPS (Design Patterns) Laboratory Work
Demonstrating structural design patterns in C#

---

## References

- Gang of Four (GoF) Design Patterns
- Head First Design Patterns
- Refactoring Guru - Design Patterns
- Microsoft C# Documentation
