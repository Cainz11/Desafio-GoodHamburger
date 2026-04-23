using GoodHamburger.Application.Menu;
using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Menu;
using GoodHamburger.Domain.Orders;
using GoodHamburger.Domain.Pricing;

namespace GoodHamburger.Application.Orders;

public static class OrderDtoMapper
{
    public static MenuItemDto ToMenuDto(MenuItem item) =>
        new()
        {
            Id = item.Id,
            Name = item.Name,
            UnitPrice = item.UnitPrice,
            Role = item.Role
        };

    public static OrderDto ToDto(Order order, IReadOnlyDictionary<Guid, MenuItem> menuById, OrderTotals totals)
    {
        var lines = order.Lines
            .Select(line =>
            {
                if (!menuById.TryGetValue(line.MenuItemId, out var item))
                {
                    throw new DomainException(
                        "menu_item_inconsistent",
                        $"Item do cardápio não está mais disponível para a linha do pedido: {line.MenuItemId}.");
                }

                return new OrderLineDto
                {
                    LineId = line.Id,
                    MenuItemId = line.MenuItemId,
                    Name = item.Name,
                    UnitPrice = item.UnitPrice,
                    Role = item.Role
                };
            })
            .ToList();

        return new OrderDto
        {
            Id = order.Id,
            Lines = lines,
            Subtotal = totals.Subtotal,
            DiscountPercent = totals.DiscountPercent,
            DiscountAmount = totals.DiscountAmount,
            Total = totals.Total
        };
    }
}
