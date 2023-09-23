namespace Bookify.Application.Abstractions.Authentication;

public record AuthenticationRequest(
    string Email,
    string Password);