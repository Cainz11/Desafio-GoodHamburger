namespace GoodHamburger.Domain.Pricing;

public sealed record OrderTotals(
    decimal Subtotal,
    decimal DiscountPercent,
    decimal DiscountAmount,
    decimal Total);
