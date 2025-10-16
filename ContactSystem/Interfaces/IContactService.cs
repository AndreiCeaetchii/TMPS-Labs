using MovieBookingSystem.Entities;

namespace MovieBookingSystem.Interfaces;

public interface IContactService
{
    Task AddContact(Contact contact);
    Task<IEnumerable<Contact>> GetAllContacts();
    Task<Contact?> SearchContact(string name);
    Task DeleteContact(string name);
}