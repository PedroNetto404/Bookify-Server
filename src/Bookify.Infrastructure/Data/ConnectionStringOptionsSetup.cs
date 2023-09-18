using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Data;

internal class ConnectionStringOptionsSetup : IConfigureOptions<ConnectionString>
{
    private readonly IConfiguration _configuration;

    public ConnectionStringOptionsSetup(IConfiguration configuration) => _configuration = configuration;

    public void Configure(ConnectionString options)
    {
        options.Value = _configuration.GetConnectionString("DefaultConnection") 
                        ?? throw new InvalidOperationException("Connection string is not set.");
    }
}
