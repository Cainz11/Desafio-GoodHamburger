using GoodHamburger.Domain.Menu;

namespace GoodHamburger.Application.Orders;

public sealed class UpsertOrderRequest
{
    public IReadOnlyList<Guid> MenuItemIds { get; init; } = Array.Empty<Guid>();
}

public sealed class OrderLineDto
{
    public Guid LineId { get; init; }
    public Guid MenuItemId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public MenuItemRole Role { get; init; }
}

public sealed class OrderDto
{
    public Guid Id { get; init; }
    public IReadOnlyList<OrderLineDto> Lines { get; init; } = Array.Empty<OrderLineDto>();
    public decimal Subtotal { get; init; }
    public decimal DiscountPercent { get; init; }
    public decimal DiscountAmount { get; init; }
    public decimal Total { get; init; }
}
