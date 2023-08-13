using Bookify.Domain.Abstractions;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging.Queries;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
    
}