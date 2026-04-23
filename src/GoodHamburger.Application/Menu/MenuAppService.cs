using GoodHamburger.Application.Abstractions;
using GoodHamburger.Application.Orders;

namespace GoodHamburger.Application.Menu;

public sealed class MenuAppService : IMenuAppService
{
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuAppService(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<IReadOnlyList<MenuItemDto>> GetMenuAsync(CancellationToken cancellationToken = default)
    {
        var items = await _menuItemRepository.GetAllAsync(cancellationToken);
        return items
            .OrderBy(i => i.Role)
            .ThenBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
            .Select(OrderDtoMapper.ToMenuDto)
            .ToList();
    }
}
