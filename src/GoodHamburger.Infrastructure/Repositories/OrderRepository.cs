using GoodHamburger.Application.Abstractions;
using GoodHamburger.Domain.Orders;
using GoodHamburger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public sealed class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<IReadOnlyList<Order>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .AsNoTracking()
            .Include(x => EF.Property<ICollection<OrderLine>>(x, "_lines"))
            .OrderByDescending(x => x.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Orders
            .Include(x => EF.Property<ICollection<OrderLine>>(x, "_lines"))
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
    {
        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
    }
}
