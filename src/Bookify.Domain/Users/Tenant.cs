using Bookify.Domain.Abstractions;
using Bookify.Domain.Users.Events;
using Bookify.Domain.Users.ValueObjects;

namespace Bookify.Domain.Users;

public class Tenant : AggregateRoot<TenantId>
{
    private Tenant(
        FirstName firstName,
        LastName lastName,
        Email email) : base(TenantId.New())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }
    
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }

    public string AuthenticationId { get; set; }
    
    public static Tenant Create(
        FirstName firstName,
        LastName lastName,
        Email email)
    {
        Tenant tenant = new(firstName, lastName, email);

        tenant.RaiseDomainEvents(new TenantCreatedEvent(tenant.Id));
        
        return tenant;
    }

    public static Tenant FromSnapshot(TenantSnapshot snapshot) =>
        new(
            new FirstName(snapshot.FirstName),
            new LastName(snapshot.LastName),
            new Email(snapshot.Email));

    public TenantSnapshot ToSnapshot() =>
        new()
        {
            TenantId = Id.Value,
            FirstName = FirstName.Value,
            LastName = LastName.Value,
            Email = Email.Value
        };
}