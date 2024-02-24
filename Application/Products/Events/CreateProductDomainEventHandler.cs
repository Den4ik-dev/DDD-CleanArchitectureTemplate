using Application.Common.Interfaces;
using Domain.Category;
using Domain.Product.Events;
using MediatR;

namespace Application.Products.Events;

public class CreateProductDomainEventHandler : INotificationHandler<CreateProductDomainEvent>
{
    private readonly IApplicationDbContext _context;

    public CreateProductDomainEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        CreateProductDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        Category category = (await _context.Categories.FindAsync(notification.CategoryId))!;

        category.AddProductId(notification.ProductId);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
