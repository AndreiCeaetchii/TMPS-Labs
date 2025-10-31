# Creational Design Patterns Implementation Report

## Purpose

This report demonstrates the implementation of four creational design patterns in the University Booking System application. Creational design patterns are focused on object creation mechanisms, making the system more flexible and reusable. The four patterns implemented in this project are:

1. **Abstract Factory Pattern** - Provides an interface for creating families of related objects without specifying their concrete classes
2. **Builder Pattern** - Separates the construction of a complex object from its representation
3. **Singleton Pattern** - Ensures a class has only one instance and provides a global point of access to it
4. **Director Pattern** - Encapsulates the construction logic for creating complex objects using the Builder pattern

## Implementation

### 1. Abstract Factory Pattern

The Abstract Factory Pattern provides an interface for creating families of related or dependent objects without specifying their concrete classes. This pattern is implemented through the `IUniversityFactory` interface and its concrete implementations.

#### Factory Interface
```csharp
public interface IUniversityFactory
{
    Room CreateLectureHall(string code);
    Room CreateLab(string code);
    Room CreateSeminarRoom(string code);
    IBookingPolicy CreatePolicy();
}
```

The interface defines methods for creating a family of related products: different room types (lecture halls, labs, seminar rooms) and university-specific booking policies.

#### Concrete Factory Implementation - Tech University
```csharp
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
```

Each concrete factory (TechUniversityFactory, MedicalUniversityFactory, LiberalArtsFactory) creates rooms with different capacities and policies that reflect the specific needs of that university type.

#### Product Families
Each factory creates:
- **Rooms** with different capacities tailored to the university type
- **Policies** with specific booking rules (e.g., Tech labs require approval, Medical facilities have different lead times)

#### Policy Interface and Implementation
```csharp
public interface IBookingPolicy
{
    bool RequiresApproval(Room room, DateTime date);
    TimeSpan MinLeadTime(Room room);
}

public class TechUPolicy : IBookingPolicy
{
    public bool RequiresApproval(Room room, DateTime date)
    {
        // Tech labs always require approval
        return room.Type.Contains("Lab");
    }

    public TimeSpan MinLeadTime(Room room)
    {
        // Labs need 3 days lead time; others 1 day
        return room.Type.Contains("Lab") ? TimeSpan.FromDays(3) : TimeSpan.FromDays(1);
    }
}
```

**Benefits:**
- Easy to add new university types by creating new factory implementations
- Ensures related products (rooms and policies) are created consistently
- Client code works with abstractions, not concrete classes
- Supports the Open/Closed Principle (open for extension, closed for modification)

---

### 2. Builder Pattern

The Builder Pattern separates the construction of a complex object from its representation, allowing the same construction process to create different representations. This is implemented through the `IBookingBuilder` interface and `BookingBuilder` class.

#### Builder Interface
```csharp
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
```

The interface provides a fluent API where each method returns `IBookingBuilder`, enabling method chaining.

#### Concrete Builder Implementation
```csharp
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

    // ... other builder methods ...

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
```

#### Usage Example
```csharp
var builder = new BookingBuilder();
var factory = new TechUniversityFactory();
var room = factory.CreateLab("L1");
var layout = new RoomLayout { Name = "Computer Lab", Seats = 20 };

var booking = builder
    .ForUniversity(factory)
    .WithRoom(room)
    .WithLayout(layout)
    .On(DateTime.Now.AddHours(3), DateTime.Now.AddHours(5))
    .RequestedBy("Lab Supervisor")
    .RequiresApproval(true)
    .Build();
```

**Benefits:**
- Provides control over the construction process
- Allows step-by-step object creation
- Fluent interface makes code more readable
- Validates required fields before creating the object
- Encapsulates complex construction logic (policy checking, conflict detection)

---

### 3. Singleton Pattern

The Singleton Pattern ensures that a class has only one instance throughout the application's lifetime and provides a global point of access to that instance. This is implemented in the `BookingRegistry` class.

#### Thread-Safe Singleton Implementation
```csharp
public class BookingRegistry
{
    private static readonly Lazy<BookingRegistry> Lazy = new(() => new BookingRegistry());
    public static BookingRegistry Instance => Lazy.Value;

    private readonly List<Booking> _bookings = new();

    private BookingRegistry() { }

    public IReadOnlyList<Booking> AllBookings => _bookings.AsReadOnly();

    public void Add(Booking booking)
    {
        _bookings.Add(booking);
    }

    public bool HasConflict(Room room, DateTime start, DateTime end)
    {
        return _bookings.Any(b =>
            b.Room.Code == room.Code &&
            start < b.End &&
            end > b.Start);
    }

    public void PrintAll()
    {
        Console.WriteLine("\n--- Current Bookings ---");
        foreach (var b in _bookings)
            Console.WriteLine(b);
    }
}
```

#### Key Features

1. **Private Constructor**: Prevents external instantiation
   ```csharp
   private BookingRegistry() { }
   ```

2. **Lazy Initialization**: Uses `Lazy<T>` for thread-safe, lazy instantiation
   ```csharp
   private static readonly Lazy<BookingRegistry> Lazy = new(() => new BookingRegistry());
   ```

3. **Global Access Point**: Single instance accessed via static property
   ```csharp
   public static BookingRegistry Instance => Lazy.Value;
   ```

#### Usage Example
```csharp
var registry = BookingRegistry.Instance;
registry.Add(booking);

if (registry.HasConflict(room, start, end))
{
    throw new InvalidOperationException("Room already booked.");
}

registry.PrintAll();
```

**Benefits:**
- Ensures single source of truth for all bookings
- Thread-safe without explicit locking (using `Lazy<T>`)
- Global access point prevents passing registry references everywhere
- Centralized conflict detection across all bookings
- Memory efficient (only one instance exists)

---

### 4. Director Pattern

The Director Pattern is an extension of the Builder pattern that encapsulates the construction logic for creating common configurations of complex objects. This is implemented in the `BookingDirector` class.

#### Director Implementation
```csharp
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
```

#### Usage Example
```csharp
var builder = new BookingBuilder();
var director = new BookingDirector();
var factory = new TechUniversityFactory();

// Create a pre-configured exam booking
var examBooking = director.CreateExamBooking(builder, factory, "A1");

// Create a pre-configured seminar booking
var seminarBooking = director.CreateSeminarBooking(builder, factory, "S1");

// Create a pre-configured lab booking
var labBooking = director.CreateLabBooking(builder, factory, "L1");
```

**Benefits:**
- Encapsulates common construction sequences
- Provides convenience methods for typical booking types
- Simplifies client code by hiding complex builder configurations
- Promotes code reuse for standard booking templates
- Easy to add new booking templates without modifying the builder

---

## Pattern Interactions

The patterns work together to create a flexible and maintainable system:

1. **Abstract Factory + Builder**: Factories create rooms and policies, which the builder uses to construct bookings
   ```csharp
   var factory = new TechUniversityFactory();
   var room = factory.CreateLab("L1");
   var booking = builder.WithRoom(room).ForUniversity(factory).Build();
   ```

2. **Builder + Singleton**: The builder uses the singleton registry to check conflicts and register bookings
   ```csharp
   var registry = BookingRegistry.Instance;
   if (registry.HasConflict(_room, _start, _end))
       throw new InvalidOperationException("Room already booked.");
   registry.Add(booking);
   ```

3. **Director + Builder + Abstract Factory**: The director orchestrates the builder and factory to create pre-configured bookings
   ```csharp
   var booking = director.CreateExamBooking(builder, factory, "A1");
   ```

## Demo Application

The `ConsoleUI` class demonstrates all patterns working together:

```csharp
public static void Run()
{
    // User selects university type (Abstract Factory)
    IUniversityFactory factory = option switch
    {
        "1" => new TechUniversityFactory(),
        "2" => new LiberalArtsFactory(),
        "3" => new MedicalUniversityFactory(),
        _ => new TechUniversityFactory()
    };

    // Create builder and director
    var builder = new BookingBuilder();
    var director = new BookingDirector();

    // User selects booking template (Director Pattern)
    Booking booking = templateChoice switch
    {
        "1" => director.CreateExamBooking(builder, factory, "A1"),
        "2" => director.CreateSeminarBooking(builder, factory, "S1"),
        "3" => director.CreateLabBooking(builder, factory, "L1"),
        _ => director.CreateExamBooking(builder, factory, "A1")
    };

    // Display results using Singleton registry
    var registry = BookingRegistry.Instance;
    registry.PrintAll();
}
```

---

## Conclusion

This University Booking System successfully demonstrates four creational design patterns:

- **Abstract Factory Pattern**: Creates families of related objects (rooms and policies) specific to each university type
- **Builder Pattern**: Constructs complex `Booking` objects step-by-step with a fluent interface
- **Singleton Pattern**: Ensures single instance of `BookingRegistry` for centralized booking management
- **Director Pattern**: Encapsulates common booking configurations for convenience

These patterns work together to create a system that is:
- **Flexible**: Easy to add new university types, room configurations, and booking templates
- **Maintainable**: Clear separation of concerns with each pattern handling specific responsibilities
- **Type-Safe**: Strong typing throughout with interface-based design
- **User-Friendly**: Fluent APIs and pre-configured templates simplify usage
- **Robust**: Centralized conflict detection and policy enforcement

The combination of these patterns demonstrates how creational patterns can work together to solve real-world object creation challenges while maintaining clean, extensible architecture.
