using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Logics.Carts;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Handlers.Carts.Commands;
using ArtGallery.Application.Logics.Carts.Queries;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CartsController : ControllerBase
{
    private readonly IMediator mediator;
    public CartsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCart(CreateCartCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateCartCommand command)
    {
        if (command != null)
        {
            command.CartId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetCartsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetCartByIdQuery(id)));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteCartCommand { Id = Id });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
