namespace GoodHamburger.Domain.Pricing;

public interface IDiscountRule
{
    bool Applies(OrderDiscountContext context);
    decimal DiscountPercent { get; }
}
