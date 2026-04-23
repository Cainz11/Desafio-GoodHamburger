using GoodHamburger.Domain.Menu;

namespace GoodHamburger.Application.Menu;

public sealed class MenuItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public MenuItemRole Role { get; init; }
}
