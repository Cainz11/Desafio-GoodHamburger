using GoodHamburger.Application.Abstractions;
using GoodHamburger.Infrastructure.Persistence;
using GoodHamburger.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sql =>
                {
                    sql.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: [10054, 10053, 40197, 40501, 40613]);
                }));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();

        return services;
    }
}
