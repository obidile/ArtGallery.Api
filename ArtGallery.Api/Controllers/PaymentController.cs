using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.Transactions.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ArtGallery.Api.Controllers;

public class PaymentController : ControllerBase
{
    private readonly IMediator mediator;
    public PaymentController(IMediator mediator)
    {
        this.mediator = mediator;
    }
    /*
      [HttpPost]
    public async Task<IActionResult> CreateArtWork([FromForm] CreateArtWorkCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
     */
    [HttpPost]
    public async Task<IActionResult> ProccessPayment(InitializeTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }
}
