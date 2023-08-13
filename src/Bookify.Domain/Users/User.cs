using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;
using Bookify.Domain.Users.ValueObjects;

namespace Bookify.Domain.Users;

public class User : Entity
{
    private User(
        Guid identifier,
        FirstName firstName,
        LastName lastName,
        Email email) : base(identifier)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    
    public static User Create(FirstName firstName, LastName lastName, Email email)
    {
        User user = new(Guid.NewGuid(), firstName, lastName, email);

        user.RaiseDomainEvents(new UserCreatedEvent(user.Identifier));
        
        return user;
    }
}


