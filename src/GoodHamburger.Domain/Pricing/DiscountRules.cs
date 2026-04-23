namespace GoodHamburger.Domain.Pricing;

public sealed class SandwichFriesSodaRule : IDiscountRule
{
    public bool Applies(OrderDiscountContext context) =>
        context.HasSandwich && context.HasFries && context.HasSoda;

    public decimal DiscountPercent => 0.20m;
}

public sealed class SandwichSodaRule : IDiscountRule
{
    public bool Applies(OrderDiscountContext context) =>
        context.HasSandwich && context.HasSoda;

    public decimal DiscountPercent => 0.15m;
}

public sealed class SandwichFriesRule : IDiscountRule
{
    public bool Applies(OrderDiscountContext context) =>
        context.HasSandwich && context.HasFries;

    public decimal DiscountPercent => 0.10m;
}
