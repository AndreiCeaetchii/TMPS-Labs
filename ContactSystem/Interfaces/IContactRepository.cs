using MovieBookingSystem.Entities;

namespace MovieBookingSystem.Interfaces;

public interface IContactRepository : IRepository<Contact>
{
    Task<Contact?> FindByName(string name);
}