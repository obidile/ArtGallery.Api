using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.ArtWorks.Command;
using ArtGallery.Application.Logics.ArtWorks.Queries;
using ArtGallery.Application.Logics.CartItems.Command;
using ArtGallery.Application.Logics.CartItems.Quries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CartItemsController : ControllerBase
{
    private readonly IMediator mediator;
    public CartItemsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCartItems([FromForm] CreateCartItemCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetCartItemsQuery()));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteCartItemCommand { Id = Id });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
