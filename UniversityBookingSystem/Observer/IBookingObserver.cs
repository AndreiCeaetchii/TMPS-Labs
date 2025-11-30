namespace UniversityBookingSystem.Observer;

public interface IBookingObserver
{
    void OnBookingCreated(BookingEventArgs args);
    void OnBookingApproved(BookingEventArgs args);
    void OnBookingCancelled(BookingEventArgs args);
    void OnBookingModified(BookingEventArgs args);
    void OnBookingCompleted(BookingEventArgs args);
}
