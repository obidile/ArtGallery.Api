using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Handlers.ArtWorks.Commands;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.ArtWorks.Command;
using ArtGallery.Application.Logics.ArtWorks.Queries;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]

public class ArtWorksController : ControllerBase
{
    private readonly IMediator mediator;
    public ArtWorksController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateArtWork([FromForm] CreateArtWorkCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromForm] UpdateArtWorkCommand command)
    {
        if (command != null)
        {
            command.ArtWorkId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] string title, string price)
    {
        return Ok(await mediator
            .Send(new GetArtWorksQuery() {title = title, price = price } ));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetArtWorkByIdQuery(id)));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
         await mediator.Send(new DeleteArtWorkCommand { Id = Id });

        return Ok(ResponseModel.Success("Removed Successfully"));
    }
}
