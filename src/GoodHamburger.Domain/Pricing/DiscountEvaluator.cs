namespace GoodHamburger.Domain.Pricing;

public sealed class DiscountEvaluator
{
    private readonly IReadOnlyList<IDiscountRule> _rules;

    public DiscountEvaluator(IEnumerable<IDiscountRule> rules)
    {
        _rules = rules?.ToList() ?? throw new ArgumentNullException(nameof(rules));
    }

    public decimal GetDiscountPercent(OrderDiscountContext context)
    {
        foreach (var rule in _rules)
        {
            if (rule.Applies(context))
            {
                return rule.DiscountPercent;
            }
        }

        return 0m;
    }
}
