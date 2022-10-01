using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Handlers.ArtWorks.Commands;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.ArtWorks.Command;
using ArtGallery.Application.Logics.ArtWorks.Queries;

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
    public async Task<IActionResult> CreateArtWork(CreateArtWorkCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateArtWorkCommand command)
    {
        if (command != null)
        {
            command.ArtWorkId = Id;
        }
       var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetArtWorksQuery()));
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

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
