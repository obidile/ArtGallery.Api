using ArtGallery.Application.Logics.Customers.Command;
using ArtGallery.Application.Logics.Customers.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator mediator;
    public CustomersController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateCustomerCommand command)
    {
        if (command != null)
        {
            command.customerId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string firstName, string mailAddress, string phoneNumber)
    {
        return Ok(await mediator
            .Send(new GetCustomersQuery() { fullName = firstName, mailAddress = mailAddress, phoneNumber = phoneNumber }));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetCustomerByIdQuery(id)));
    }
}
