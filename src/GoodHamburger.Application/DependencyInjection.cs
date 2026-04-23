using FluentValidation;
using GoodHamburger.Application.Menu;
using GoodHamburger.Application.Orders;
using GoodHamburger.Domain.Pricing;
using Microsoft.Extensions.DependencyInjection;

namespace GoodHamburger.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UpsertOrderRequestValidator>();

        services.AddSingleton(_ => new DiscountEvaluator(
            new IDiscountRule[]
            {
                new SandwichFriesSodaRule(),
                new SandwichSodaRule(),
                new SandwichFriesRule()
            }));

        services.AddScoped<IOrdersAppService, OrdersAppService>();
        services.AddScoped<IMenuAppService, MenuAppService>();

        return services;
    }
}
