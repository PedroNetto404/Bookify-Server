using Bookify.Application.Abstractions.Messaging.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Namespace;
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{

    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing {CommandName}", name);

            var result = await next();

            _logger.LogInformation("Command {CommandName} executed", name);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Command {CommandName} processing failed", name);

            throw;
        }
    }
}
