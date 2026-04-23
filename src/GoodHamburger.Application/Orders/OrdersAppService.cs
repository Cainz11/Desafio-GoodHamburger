using FluentValidation;
using GoodHamburger.Application.Abstractions;
using GoodHamburger.Application.Exceptions;
using GoodHamburger.Domain.Orders;
using GoodHamburger.Domain.Pricing;

namespace GoodHamburger.Application.Orders;

public sealed class OrdersAppService : IOrdersAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMenuItemRepository _menuItemRepository;
    private readonly DiscountEvaluator _discountEvaluator;
    private readonly IValidator<UpsertOrderRequest> _validator;

    public OrdersAppService(
        IOrderRepository orderRepository,
        IMenuItemRepository menuItemRepository,
        DiscountEvaluator discountEvaluator,
        IValidator<UpsertOrderRequest> validator)
    {
        _orderRepository = orderRepository;
        _menuItemRepository = menuItemRepository;
        _discountEvaluator = discountEvaluator;
        _validator = validator;
    }

    public async Task<IReadOnlyList<OrderDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.ListAsync(cancellationToken);
        var result = new List<OrderDto>();
        foreach (var order in orders)
        {
            result.Add(await MapOrderAsync(order, cancellationToken));
        }

        return result;
    }

    public async Task<OrderDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException("order_not_found", "Pedido não encontrado.");
        }

        return await MapOrderAsync(order, cancellationToken);
    }

    public async Task<OrderDto> CreateAsync(UpsertOrderRequest request, CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var menuById = await _menuItemRepository.GetByIdsAsync(request.MenuItemIds, cancellationToken);
        var order = Order.Create(Guid.NewGuid());
        order.ReplaceLines(request.MenuItemIds, menuById);

        await _orderRepository.AddAsync(order, cancellationToken);
        return await MapOrderAsync(order, cancellationToken);
    }

    public async Task<OrderDto> UpdateAsync(
        Guid id,
        UpsertOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException("order_not_found", "Pedido não encontrado.");
        }

        var menuById = await _menuItemRepository.GetByIdsAsync(request.MenuItemIds, cancellationToken);
        order.ReplaceLines(request.MenuItemIds, menuById);

        await _orderRepository.UpdateAsync(order, cancellationToken);
        return await MapOrderAsync(order, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order is null)
        {
            throw new NotFoundException("order_not_found", "Pedido não encontrado.");
        }

        await _orderRepository.DeleteAsync(order, cancellationToken);
    }

    private async Task<OrderDto> MapOrderAsync(Order order, CancellationToken cancellationToken)
    {
        var ids = order.Lines.Select(l => l.MenuItemId).Distinct().ToArray();
        var menuById = await _menuItemRepository.GetByIdsAsync(ids, cancellationToken);
        var totals = OrderPricing.Calculate(order, menuById, _discountEvaluator);
        return OrderDtoMapper.ToDto(order, menuById, totals);
    }
}
