using GoodHamburger.Domain.Orders;

namespace GoodHamburger.Application.Abstractions;

public interface IOrderRepository
{
    Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default);

    Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);

    Task DeleteAsync(Order order, CancellationToken cancellationToken = default);
}
