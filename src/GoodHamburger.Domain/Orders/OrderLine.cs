namespace GoodHamburger.Domain.Orders;

public sealed class OrderLine
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid MenuItemId { get; private set; }

    private OrderLine()
    {
    }

    public static OrderLine Create(Guid id, Guid orderId, Guid menuItemId)
    {
        return new OrderLine
        {
            Id = id,
            OrderId = orderId,
            MenuItemId = menuItemId
        };
    }
}
