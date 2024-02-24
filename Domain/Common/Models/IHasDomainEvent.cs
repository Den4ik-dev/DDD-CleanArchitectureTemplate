namespace Domain.Common.Models;

public interface IHasDomainEvent
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
