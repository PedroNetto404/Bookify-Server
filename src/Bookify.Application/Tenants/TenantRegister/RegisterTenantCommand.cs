using Bookify.Application.Abstractions.Messaging.Commands;

namespace Bookify.Application.Tenants.TenantRegister;

public record RegisterTenantCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand<Guid>; 