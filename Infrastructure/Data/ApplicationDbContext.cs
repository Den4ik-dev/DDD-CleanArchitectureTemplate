using System.Reflection;
using Application.Common.Interfaces;
using Domain.Category;
using Domain.Product;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly DispatchDomainEventInterceptor _dispatchDomainEventInterceptor;

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        DispatchDomainEventInterceptor dispatchDomainEventInterceptor
    )
        : base(options)
    {
        _dispatchDomainEventInterceptor = dispatchDomainEventInterceptor;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_dispatchDomainEventInterceptor);

        base.OnConfiguring(optionsBuilder);
    }
}
