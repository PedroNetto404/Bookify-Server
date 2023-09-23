namespace Bookify.Application.Abstractions.Authentication.Register;

public interface IAuthRegistrationService
{
    Task<RegistrationResponse?> RegisterAsync(
        RegistrationRequest request,
        CancellationToken cancellationToken = default);
}