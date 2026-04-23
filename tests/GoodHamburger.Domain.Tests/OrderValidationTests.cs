using GoodHamburger.Domain.Exceptions;
using GoodHamburger.Domain.Menu;
using GoodHamburger.Domain.Orders;

namespace GoodHamburger.Domain.Tests;

public sealed class OrderValidationTests
{
    private static readonly Guid Burger = Guid.Parse("20000000-0000-0000-0000-000000000001");
    private static readonly Guid Egg = Guid.Parse("20000000-0000-0000-0000-000000000002");

    private static readonly IReadOnlyDictionary<Guid, MenuItem> Menu = new Dictionary<Guid, MenuItem>
    {
        [Burger] = MenuItem.Create(Burger, "X Burger", 5.00m, MenuItemRole.Sandwich),
        [Egg] = MenuItem.Create(Egg, "X Egg", 4.50m, MenuItemRole.Sandwich)
    };

    [Fact]
    public void Dois_sanduiches_disparam_erro_de_categoria_duplicada()
    {
        var order = Order.Create(Guid.NewGuid());

        var ex = Assert.Throws<DomainException>(() => order.ReplaceLines(new[] { Burger, Egg }, Menu));

        Assert.Equal("duplicate_category", ex.ErrorCode);
    }

    [Fact]
    public void Mesmo_item_duas_vezes_dispara_erro_de_duplicidade()
    {
        var order = Order.Create(Guid.NewGuid());

        var ex = Assert.Throws<DomainException>(() => order.ReplaceLines(new[] { Burger, Burger }, Menu));

        Assert.Equal("duplicate_menu_item", ex.ErrorCode);
    }

    [Fact]
    public void Id_inexistente_dispara_erro_de_item_invalido()
    {
        var order = Order.Create(Guid.NewGuid());
        var missing = Guid.Parse("30000000-0000-0000-0000-000000000099");

        var ex = Assert.Throws<DomainException>(() => order.ReplaceLines(new[] { Burger, missing }, Menu));

        Assert.Equal("invalid_menu_item", ex.ErrorCode);
    }
}
