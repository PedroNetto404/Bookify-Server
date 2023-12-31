using Bookify.Domain.Utility.Results;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging.Commands;

public interface ICommand : IRequest<Result>, IBaseCommand
{
}


public interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand
{
}

public interface IBaseCommand {}