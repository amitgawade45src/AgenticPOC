using BankService.Domain.Common;
using MediatR;

namespace BankService.Application.UseCases.Common;

internal static class DomainEventPublisher
{
    public static async Task PublishAndClearAsync(
        AggregateRoot aggregate,
        IPublisher publisher,
        CancellationToken cancellationToken)
    {
        foreach (var domainEvent in aggregate.DomainEvents)
        {
            await publisher.Publish(domainEvent, cancellationToken);
        }

        aggregate.ClearDomainEvents();
    }
}
