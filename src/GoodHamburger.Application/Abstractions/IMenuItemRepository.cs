using GoodHamburger.Domain.Menu;

namespace GoodHamburger.Application.Abstractions;

public interface IMenuItemRepository
{
    Task<IReadOnlyList<MenuItem>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, MenuItem>> GetByIdsAsync(
        IEnumerable<Guid> ids,
        CancellationToken cancellationToken = default);
}
