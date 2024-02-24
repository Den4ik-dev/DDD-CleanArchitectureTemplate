using Domain.Category;
using Domain.Product;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<Category> Categories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
