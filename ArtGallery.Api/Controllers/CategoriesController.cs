using Microsoft.AspNetCore.Mvc;
using MediatR;
using ArtGallery.Application.Logics.Categories;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Handlers.Categories.Commands;
using ArtGallery.Application.Logics.Users.Queries;
using ArtGallery.Application.Logics.Categories.Queries;

namespace ArtGallery.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//Category
public class CategoriesController : ControllerBase
{
    private readonly IMediator mediator;
    public CategoriesController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] long Id, [FromBody] UpdateCategoryCommand command)
    {
        if (command != null)
        {
            command.CategoryId = Id;
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }


    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await mediator.Send(new GetCategoriesQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] long id)
    {
        return Ok(await mediator.Send(new GetCategoryByIdQuery(id)));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] long Id)
    {
        await mediator.Send(new DeleteCategoryCommand { Id = Id });

        return Ok(ResponseModel.Success("Deleted Successfully"));
    }
}
