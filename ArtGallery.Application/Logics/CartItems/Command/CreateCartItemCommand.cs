using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.CartItems.Command;

public class CreateCartItemCommand : IRequest<string>
{
    public long CartId { get; set; }
    public long ArtworkId { get; set; }
    public virtual ArtWork ArtWork { get; set; }
}

public class CreateCartItemCommandValidator : AbstractValidator<CreateCartItemCommand>
{
    public CreateCartItemCommandValidator()
    {
        RuleFor(x => x.ArtworkId).Empty();

    }
}
public class CreateCartitemCommandHandler : IRequestHandler<CreateCartItemCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CreateCartitemCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<string> Handle(CreateCartItemCommand request, CancellationToken cancellationToken)
    {
        var cart = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ArtworkId);
        if (cart == null)
        {
            return "No artwork has been added to cart";
        }

        var model = new CartItem()
        {
            CartId = request.CartId,
            ArtworkId = request.ArtworkId,
            ArtWork = request.ArtWork
        };

        _dbContext.CartItems.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return "Your cart has been added to cartItmes";
    }


}