using MovieBookingSystem.Entities;
using MovieBookingSystem.Interfaces;

namespace MovieBookingSystem.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;
    private readonly INotificationService _notificationService;

    public ContactService(IContactRepository repository, INotificationService notificationService)
    {
        _repository = repository;
        _notificationService = notificationService;
    }

    public Task AddContact(Contact contact)
    {
        _repository.Add(contact);
        _notificationService.SendNotification(contact.Email, $"New contact '{contact.Name}' added.");
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Contact>> GetAllContacts() => _repository.GetAll();

    public Task<Contact?> SearchContact(string name) => _repository.FindByName(name);

    public async Task DeleteContact(string name)
    {
        var contact = await _repository.FindByName(name);
        if (contact != null)
        {
            await _repository.Remove(contact.Email);
            _notificationService.SendNotification(contact.Email, $"Contact '{contact.Name}' has been removed.");
        }
    }
}