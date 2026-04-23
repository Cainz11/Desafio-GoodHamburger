using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Menu;

namespace GoodHamburger.Domain.Orders;

public sealed class Order
{
    private readonly List<OrderLine> _lines = new();

    public Guid Id { get; private set; }
    public IReadOnlyCollection<OrderLine> Lines => _lines;

    private Order()
    {
    }

    public static Order Create(Guid id)
    {
        return new Order { Id = id };
    }

    public void ReplaceLines(IEnumerable<Guid> menuItemIds, IReadOnlyDictionary<Guid, MenuItem> menuItemsById)
    {
        ArgumentNullException.ThrowIfNull(menuItemIds);
        ArgumentNullException.ThrowIfNull(menuItemsById);

        var ids = menuItemIds.ToList();
        var distinctIds = ids.Distinct().ToList();
        if (ids.Count != distinctIds.Count)
        {
            throw new DomainException(
                "duplicate_menu_item",
                "Itens duplicados não são permitidos no mesmo pedido.");
        }

        var resolved = new List<MenuItem>();
        foreach (var menuItemId in ids)
        {
            if (!menuItemsById.TryGetValue(menuItemId, out var item))
            {
                throw new DomainException(
                    "invalid_menu_item",
                    $"Item do cardápio não encontrado: {menuItemId}.");
            }

            resolved.Add(item);
        }

        var byRole = resolved.GroupBy(i => i.Role).ToList();
        foreach (var group in byRole)
        {
            if (group.Count() > 1)
            {
                var roleLabel = group.Key switch
                {
                    MenuItemRole.Sandwich => "sanduíche",
                    MenuItemRole.Fries => "acompanhamento (batata)",
                    MenuItemRole.Soda => "refrigerante",
                    _ => "categoria"
                };

                throw new DomainException(
                    "duplicate_category",
                    $"O pedido aceita apenas um {roleLabel}. Remova os itens duplicados dessa categoria.");
            }
        }

        _lines.Clear();
        foreach (var menuItemId in ids)
        {
            _lines.Add(OrderLine.Create(Guid.NewGuid(), Id, menuItemId));
        }
    }
}
