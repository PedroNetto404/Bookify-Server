namespace Bookify.Domain.Tenants.ValueObjects;

public record LastName(string Value)
{
    public const int MaxLength = 255;
}
