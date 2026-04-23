namespace GoodHamburger.Application.Orders;

public interface IOrdersAppService
{
    Task<IReadOnlyList<OrderDto>> ListAsync(CancellationToken cancellationToken = default);

    Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<OrderDto> CreateAsync(UpsertOrderRequest request, CancellationToken cancellationToken = default);

    Task<OrderDto> UpdateAsync(Guid id, UpsertOrderRequest request, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
