using Bookify.Domain.Users.ValueObjects;

namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Users.ValueObjects.Email recipient, string subject, string body); 
}