using Microsoft.AspNetCore.Mvc;
using ArtGallery.Application.Logics.Users;
using MediatR;
using ArtGallery.Application.Handlers.Users.Commands;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.Users.Queries;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class UsersController : ControllerBase
{
    private readonly IMediator mediator;
    public UsersController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long UserId, [FromBody] UpdateUserCommand command)
    {
        if (command != null)
        {
            command.Id = UserId;
        }
       var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetUsersQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetUserByIdQuery(id)));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long UserId)
    {
         await mediator.Send(new DeleteUserCommand { UserId = UserId });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
