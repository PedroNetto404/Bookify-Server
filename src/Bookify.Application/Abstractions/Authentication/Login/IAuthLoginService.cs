namespace Bookify.Application.Abstractions.Authentication;

public interface IAuthLoginService
{
    Task<AuthencationResponse> LoginAsync(AuthenticationRequest request, CancellationToken cancellationToken = default);
}