using Bookify.Application.Tenants.TenantRegister;
using Bookify.Presentation.Api.Endpoints.Abstractions.ApiController;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Presentation.Api.Endpoints.Tenants;

public class TenantsController : ApiController
{
    public TenantsController(ISender sender) : base(sender)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(RegisterTenantRequest request)
    {
        var command = (RegisterTenantCommand) request;
        var result = await Sender.Send(command);

        return FromResult(result);
    }

}

public record RegisterTenantRequest
{
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public string Password { get; init; } = null!;
    
    public static implicit operator RegisterTenantCommand(RegisterTenantRequest request) => new(
        request.FirstName,
        request.LastName,
        request.Email,
        request.Password);
}