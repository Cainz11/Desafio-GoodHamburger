using GoodHamburger.Application.Orders;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IOrdersAppService ordersAppService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<OrderDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<OrderDto>>> ListAsync(CancellationToken cancellationToken)
    {
        var orders = await ordersAppService.ListAsync(cancellationToken);
        return Ok(orders);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await ordersAppService.GetByIdAsync(id, cancellationToken);
        return Ok(order);
    }

    [HttpPost]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<OrderDto>> CreateAsync(
        [FromBody] UpsertOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await ordersAppService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetByIdAsync), new { id = order.Id }, order);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderDto>> UpdateAsync(
        Guid id,
        [FromBody] UpsertOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await ordersAppService.UpdateAsync(id, request, cancellationToken);
        return Ok(order);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await ordersAppService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
