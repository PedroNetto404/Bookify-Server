namespace Bookify.Application.Abstractions.Messaging.Clock;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}