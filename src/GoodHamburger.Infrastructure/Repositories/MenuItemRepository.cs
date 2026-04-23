using GoodHamburger.Application.Abstractions;
using GoodHamburger.Domain.Menu;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public sealed class MenuItemRepository(AppDbContext context) : IMenuItemRepository
{
    public async Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.MenuItems
            .AsNoTracking()
            .OrderBy(x => x.Role)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyDictionary<Guid, MenuItem>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default)
    {
        var idList = ids.Distinct().ToList();
        if (idList.Count == 0)
        {
            return new Dictionary<Guid, MenuItem>();
        }

        var items = await context.MenuItems
            .AsNoTracking()
            .Where(x => idList.Contains(x.Id))
            .ToListAsync(cancellationToken);

        return items.ToDictionary(x => x.Id);
    }
}
