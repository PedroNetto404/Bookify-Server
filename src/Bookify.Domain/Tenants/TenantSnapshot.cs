namespace Bookify.Domain.Tenants;

public record TenantSnapshot
{
    public Guid TenantId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string AuthenticationId { get; set; } = null!;
}