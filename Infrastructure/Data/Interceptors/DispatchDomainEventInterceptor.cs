using Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data.Interceptors;

public class DispatchDomainEventInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DispatchDomainEventInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result
    )
    {
        DispatchDomainEvent(eventData.Context).GetAwaiter().GetResult();

        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken
    )
    {
        await DispatchDomainEvent(eventData.Context);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvent(DbContext? dbContext)
    {
        if (dbContext is null)
            return;

        List<IHasDomainEvent> entities = dbContext
            .ChangeTracker.Entries<IHasDomainEvent>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        List<IDomainEvent> domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (IDomainEvent domainEvent in domainEvents)
            await _publisher.Publish(domainEvent);
    }
}
