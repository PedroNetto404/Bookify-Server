using Bookify.Domain.Utility.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Presentation.Api.Endpoints.Abstractions.ApiController;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult FromResult<T>(Result<T> result)
    {
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }
}