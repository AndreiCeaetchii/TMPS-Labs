using MovieBookingSystem.Entities;
using MovieBookingSystem.Interfaces;
using MovieBookingSystem.Services;

namespace MovieBookingSystem;

class Program
{
    static async Task Main()
    {
        IContactRepository repository = new InMemoryContactRepository();
        INotificationService notification = new ConsoleNotificationService();
        IContactService contactService = new ContactService(repository, notification);

        await contactService.AddContact(new Contact("Alice", "alice@email.com", "123456"));
        await contactService.AddContact(new Contact("Bob", "bob@email.com", "987654"));

        var contacts = await contactService.GetAllContacts();
        foreach (var contact in contacts)
            Console.WriteLine(contact);

        await contactService.DeleteContact("Alice");
    }
}