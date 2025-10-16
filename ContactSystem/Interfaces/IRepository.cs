namespace MovieBookingSystem.Interfaces;

public interface IRepository<T>
{
    Task Add(T item);
    Task<IEnumerable<T>> GetAll();
    Task<T?> FindById(string id);
    Task Remove(string id);
}