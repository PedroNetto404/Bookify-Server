namespace Bookify.Application.Abstractions.Authentication.Register;

public record RegistrationRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);