using GoodHamburger.Domain.Menu;
using GoodHamburger.Domain.Orders;

namespace GoodHamburger.Domain.Pricing;

public static class OrderPricing
{
    private static readonly DiscountEvaluator DefaultEvaluator = new(
        new IDiscountRule[]
        {
            new SandwichFriesSodaRule(),
            new SandwichSodaRule(),
            new SandwichFriesRule()
        });

    public static OrderDiscountContext BuildContext(
        Order order,
        IReadOnlyDictionary<Guid, MenuItem> menuItemsById)
    {
        var hasSandwich = false;
        var hasFries = false;
        var hasSoda = false;

        foreach (var line in order.Lines)
        {
            if (!menuItemsById.TryGetValue(line.MenuItemId, out var item))
            {
                continue;
            }

            switch (item.Role)
            {
                case MenuItemRole.Sandwich:
                    hasSandwich = true;
                    break;
                case MenuItemRole.Fries:
                    hasFries = true;
                    break;
                case MenuItemRole.Soda:
                    hasSoda = true;
                    break;
            }
        }

        return new OrderDiscountContext(hasSandwich, hasFries, hasSoda);
    }

    public static OrderTotals Calculate(Order order, IReadOnlyDictionary<Guid, MenuItem> menuItemsById)
    {
        return Calculate(order, menuItemsById, DefaultEvaluator);
    }

    public static OrderTotals Calculate(
        Order order,
        IReadOnlyDictionary<Guid, MenuItem> menuItemsById,
        DiscountEvaluator evaluator)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(menuItemsById);
        ArgumentNullException.ThrowIfNull(evaluator);

        var subtotal = 0m;
        foreach (var line in order.Lines)
        {
            if (!menuItemsById.TryGetValue(line.MenuItemId, out var item))
            {
                continue;
            }

            subtotal += item.UnitPrice;
        }

        subtotal = decimal.Round(subtotal, 2, MidpointRounding.AwayFromZero);

        var context = BuildContext(order, menuItemsById);
        var discountPercent = evaluator.GetDiscountPercent(context);
        var discountAmount = decimal.Round(subtotal * discountPercent, 2, MidpointRounding.AwayFromZero);
        var total = decimal.Round(subtotal - discountAmount, 2, MidpointRounding.AwayFromZero);

        return new OrderTotals(subtotal, discountPercent, discountAmount, total);
    }
}
