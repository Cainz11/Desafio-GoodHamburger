namespace GoodHamburger.Application.Menu;

public interface IMenuAppService
{
    Task<IReadOnlyList<MenuItemDto>> GetMenuAsync(CancellationToken cancellationToken = default);
}
