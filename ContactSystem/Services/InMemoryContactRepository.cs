using MovieBookingSystem.Entities;
using MovieBookingSystem.Interfaces;

namespace MovieBookingSystem.Services;

public class InMemoryContactRepository : IContactRepository
{
    private readonly List<Contact> _contacts = new();

    public Task Add(Contact contact)
    {
        _contacts.Add(contact);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Contact>> GetAll()
        => Task.FromResult(_contacts.AsEnumerable());

    public Task<Contact?> FindById(string id)
    {
        var contact = _contacts
            .FirstOrDefault(c => c.Email.Equals(id, System.StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(contact);
    }

    public async Task Remove(string id)
    {
        var contact = await FindById(id);
        if (contact != null)
            _contacts.Remove(contact);
    }

    public Task<Contact?> FindByName(string name)
    {
        var contact = _contacts
            .FirstOrDefault(c => c.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(contact);
    }
}