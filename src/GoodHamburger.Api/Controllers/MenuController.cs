using GoodHamburger.Application.Menu;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/menu")]
public sealed class MenuController(IMenuAppService menuAppService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MenuItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MenuItemDto>>> GetAsync(CancellationToken cancellationToken)
    {
        var menu = await menuAppService.GetMenuAsync(cancellationToken);
        return Ok(menu);
    }
}
