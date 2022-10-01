using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Handlers.Carts.Commands;
using ArtGallery.Application.Logics.Auth.Commands;
using ArtGallery.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace ArtGallery.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase //ApiControllerBase
{
    private readonly IMediator mediator;
    public AuthController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var login = await mediator.Send(command);
        return Ok(login);
    }

    [Produces("application/json")]
    [HttpPost("Logout")]
    public async Task<IActionResult> Logout([FromRoute] long Id, [FromBody] LogoutCommand command)
    {
        if (User.Identity.IsAuthenticated)
        {
            command.UserId = Id;
        }
        var res = await mediator.Send(command);
        //await _hub.Clients.All.SendAsync("forbidden", new { token = command.RefreshToken });
        return Ok(res);
    }
}