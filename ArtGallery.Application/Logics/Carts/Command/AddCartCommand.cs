using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.Carts;

public class AddCartCommand : IRequest<ResponseModel>
{
    public string CartSessionKey { get; set; }
    public long? UserId { get; set; }
    public long ArtWorkId { get; set; }
    public virtual ArtWork ArtWork { get; set; }
    
}

public class AddCartCommandValidator : AbstractValidator<AddCartCommand>
{
    public AddCartCommandValidator()
    {
        RuleFor(x => x.ArtWorkId).Empty();
    }
}

public class AddCartCommandHandler : IRequestHandler<AddCartCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public AddCartCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(AddCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.ArtWorks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.ArtWorkId);
        if (product == null)
        {
            return ResponseModel.Failure("This artwork does not exist.");
        }

        //if (!product.IsActive)
        //{
        //    return ResponseModel.Failure("This artwork is currently not active.");
        //}

        if (product.Quantity == 0)
        {
            return ResponseModel.Failure("This artwork is out of stock.");
        }

        Cart cart = null;

        if (request.UserId.HasValue && request.UserId.Value > 0)
        {
            cart = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == request.UserId);
        }
        else if (!string.IsNullOrEmpty(request.CartSessionKey))
        {
            cart = await _dbContext.Carts.AsNoTracking().FirstOrDefaultAsync(x => x.CartSessionKey == request.CartSessionKey);
        }

        if (cart == null)
        {
            cart = new Cart();
        }

        cart.UserId = request.UserId;
        cart.CartSessionKey = request.CartSessionKey ?? Guid.NewGuid().ToString();
        cart.ArtWorkId = product.Id;
        cart.Quantity = cart.Quantity + 1;
        _dbContext.Carts.Add(cart);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<CartModel>.Success(_mapper.Map<CartModel>(cart), "ArtWork Was successfully added to Cart");
    }
}
