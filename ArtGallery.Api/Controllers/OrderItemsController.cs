using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Handlers.OrderItems.Commands;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.OrderItems.Command;
using ArtGallery.Application.Logics.OrderItems;
using ArtGallery.Application.Logics.Users.Queries;
using ArtGallery.Application.Logics.OrderItems.Queries;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OrderItemsController : ControllerBase
{
    private readonly IMediator mediator;
    public OrderItemsController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrderItem([FromForm] CreateOrderItemCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromForm] UpdateOrderItemCommand command)
    {
        if (command != null)
        {
            command.orderId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetOrderItemsQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetOrderItemByIdQuery(id)));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteOrderItemCommand { Id = Id });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
