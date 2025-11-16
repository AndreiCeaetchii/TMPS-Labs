namespace UniversityBookingSystem.Decorators;

/// <summary>
/// Concrete decorator that adds recording and streaming services to a booking.
/// </summary>
public class RecordingDecorator : BookingDecorator
{
    private readonly bool _liveStreaming;
    private readonly bool _recording;
    private const decimal RecordingCost = 75.0m;
    private const decimal StreamingCost = 100.0m;

    public RecordingDecorator(IBookingComponent booking, bool recording = true, bool liveStreaming = false)
        : base(booking)
    {
        _recording = recording;
        _liveStreaming = liveStreaming;
    }

    public override string GetDescription()
    {
        var services = new List<string>();
        if (_recording) services.Add("Recording");
        if (_liveStreaming) services.Add("Live Streaming");

        return $"{base.GetDescription()} + {string.Join(" & ", services)}";
    }

    public override decimal GetCost()
    {
        var additionalCost = 0m;
        if (_recording) additionalCost += RecordingCost;
        if (_liveStreaming) additionalCost += StreamingCost;

        return base.GetCost() + additionalCost;
    }

    public override void DisplayDetails()
    {
        base.DisplayDetails();
        if (_recording)
            Console.WriteLine($"  + Recording Service (${RecordingCost:F2})");
        if (_liveStreaming)
            Console.WriteLine($"  + Live Streaming Service (${StreamingCost:F2})");
    }
}
