using Bookify.Domain.Utility.Results;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging.Queries;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}