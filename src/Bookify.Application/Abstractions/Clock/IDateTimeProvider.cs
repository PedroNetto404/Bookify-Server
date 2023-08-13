namespace Bookify.Application.Abstractions.Messaging.Clock;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}