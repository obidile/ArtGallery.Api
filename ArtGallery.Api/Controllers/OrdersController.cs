using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Logics.Orders;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Handlers.Orders.Commands;
using ArtGallery.Application.Logics.Orders.Command;
using ArtGallery.Application.Logics.Users.Queries;
using ArtGallery.Application.Logics.Orders.Queries;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class OrdersController : ControllerBase
{
    private readonly IMediator mediator;
    public OrdersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromForm] CreateOrderCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromForm] UpdateOrderCommand command)
    {
        if (command != null)
        {
            command.OrderId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetOrdersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetOrderByIdQuery(id)));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteOrderCommand { Id = Id });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
