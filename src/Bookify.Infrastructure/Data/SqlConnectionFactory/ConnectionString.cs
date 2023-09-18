namespace Bookify.Infrastructure.Data.SqlConnectionFactory;

internal record ConnectionString
{
    public string Value { get; set; } = null!;
}
