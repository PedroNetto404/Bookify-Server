namespace Bookify.Domain.Tenants.ValueObjects;

public record FirstName(string Value)
{
    public const int MaxLength = 255;
}