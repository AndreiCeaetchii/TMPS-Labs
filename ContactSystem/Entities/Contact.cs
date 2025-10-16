namespace MovieBookingSystem.Entities;

public class Contact
{
    public Contact(string name, string email, string phone)
    {
        Name = name;
        Email = email;
        Phone = phone;
    }

    public string Name { get; }
    public string Email { get; }
    public string Phone { get; }

    public override string ToString()
    {
        return $"{Name} | {Email} | {Phone}";
    }
}