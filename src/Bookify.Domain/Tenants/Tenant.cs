using Bookify.Domain.Abstractions;
using Bookify.Domain.Tenants.Events;
using Bookify.Domain.Tenants.ValueObjects;

namespace Bookify.Domain.Tenants;

public sealed class Tenant : AggregateRoot<TenantId>
{
    private Tenant(
        FirstName firstName,
        LastName lastName,
        Email email,
        string authenticationId) : base(TenantId.New())
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        AuthenticationId = authenticationId;
    }
    
    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string AuthenticationId { get; private set; }
    
    public static Tenant Create(
        FirstName firstName,
        LastName lastName,
        Email email,
        string authenticationId)
    {
        Tenant tenant = new(firstName, lastName, email, authenticationId);

        tenant.RaiseDomainEvents(new TenantCreatedEvent(tenant.Id));
        
        return tenant;
    }

    public static Tenant FromSnapshot(TenantSnapshot snapshot) =>
        new(
            new FirstName(snapshot.FirstName),
            new LastName(snapshot.LastName),
            new Email(snapshot.Email),
            snapshot.AuthenticationId);

    public TenantSnapshot ToSnapshot() =>
        new()
        {
            TenantId = Id.Value,
            FirstName = FirstName.Value,
            LastName = LastName.Value,
            Email = Email.Value,
            AuthenticationId = AuthenticationId
        };
}