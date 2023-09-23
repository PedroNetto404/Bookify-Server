namespace Bookify.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(Domain.Tenants.ValueObjects.Email recipient, string subject, string body); 
}