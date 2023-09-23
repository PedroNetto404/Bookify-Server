using Bookify.Domain.Utility.Results;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}