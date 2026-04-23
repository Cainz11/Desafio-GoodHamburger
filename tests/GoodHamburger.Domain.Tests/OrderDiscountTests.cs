using GoodHamburger.Domain.Menu;
using GoodHamburger.Domain.Orders;
using GoodHamburger.Domain.Pricing;

namespace GoodHamburger.Domain.Tests;

public sealed class OrderDiscountTests
{
    private static readonly Guid Burger = Guid.Parse("10000000-0000-0000-0000-000000000001");
    private static readonly Guid Fries = Guid.Parse("10000000-0000-0000-0000-000000000002");
    private static readonly Guid Soda = Guid.Parse("10000000-0000-0000-0000-000000000003");

    private static readonly IReadOnlyDictionary<Guid, MenuItem> Menu = new Dictionary<Guid, MenuItem>
    {
        [Burger] = MenuItem.Create(Burger, "X Burger", 5.00m, MenuItemRole.Sandwich),
        [Fries] = MenuItem.Create(Fries, "Batata frita", 2.00m, MenuItemRole.Fries),
        [Soda] = MenuItem.Create(Soda, "Refrigerante", 2.50m, MenuItemRole.Soda)
    };

    private static readonly DiscountEvaluator Evaluator = new(
        new IDiscountRule[]
        {
            new SandwichFriesSodaRule(),
            new SandwichSodaRule(),
            new SandwichFriesRule()
        });

    [Fact]
    public void Combo_completo_aplica_20_porcento()
    {
        var order = Order.Create(Guid.NewGuid());
        order.ReplaceLines(new[] { Burger, Fries, Soda }, Menu);

        var totals = OrderPricing.Calculate(order, Menu, Evaluator);

        Assert.Equal(9.50m, totals.Subtotal);
        Assert.Equal(0.20m, totals.DiscountPercent);
        Assert.Equal(1.90m, totals.DiscountAmount);
        Assert.Equal(7.60m, totals.Total);
    }

    [Fact]
    public void Sanduiche_e_refrigerante_aplica_15_porcento()
    {
        var order = Order.Create(Guid.NewGuid());
        order.ReplaceLines(new[] { Burger, Soda }, Menu);

        var totals = OrderPricing.Calculate(order, Menu, Evaluator);

        Assert.Equal(7.50m, totals.Subtotal);
        Assert.Equal(0.15m, totals.DiscountPercent);
        Assert.Equal(1.13m, totals.DiscountAmount);
        Assert.Equal(6.37m, totals.Total);
    }

    [Fact]
    public void Sanduiche_e_batata_aplica_10_porcento()
    {
        var order = Order.Create(Guid.NewGuid());
        order.ReplaceLines(new[] { Burger, Fries }, Menu);

        var totals = OrderPricing.Calculate(order, Menu, Evaluator);

        Assert.Equal(7.00m, totals.Subtotal);
        Assert.Equal(0.10m, totals.DiscountPercent);
        Assert.Equal(0.70m, totals.DiscountAmount);
        Assert.Equal(6.30m, totals.Total);
    }

    [Fact]
    public void Sem_desconto_quando_sem_combo_valido()
    {
        var order = Order.Create(Guid.NewGuid());
        order.ReplaceLines(new[] { Fries, Soda }, Menu);

        var totals = OrderPricing.Calculate(order, Menu, Evaluator);

        Assert.Equal(4.50m, totals.Subtotal);
        Assert.Equal(0m, totals.DiscountPercent);
        Assert.Equal(0m, totals.DiscountAmount);
        Assert.Equal(4.50m, totals.Total);
    }
}
