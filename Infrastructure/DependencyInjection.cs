using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<DispatchDomainEventInterceptor>();

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(
            options => options.UseSqlServer(configuration.GetConnectionString("Default"))
        );

        return services;
    }
}
