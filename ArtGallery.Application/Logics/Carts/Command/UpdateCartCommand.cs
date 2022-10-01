using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.Carts.Commands;

public partial class UpdateCartCommand : IRequest<ResponseModel>
{
    public long CartId { get; set; }
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public long Discount { get; set; }
}

public class UpdateCartCommandValidator : AbstractValidator<UpdateCartCommand>
{
    public UpdateCartCommandValidator()
    {
        RuleFor(v => v.CartId).NotEmpty();
    }
}

public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateCartCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _dbContext.Carts.FirstOrDefaultAsync(x => x.Id == request.CartId);

        if (cart == null)
        {
            return ResponseModel.Failure("Cart was not found");
        }

        cart.ProductId = request.ProductId;
        cart.Quantity = request.Quantity;
        cart.Discount = request.Discount;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<CartModel>.Success(_mapper.Map<CartModel>(cart), "Cart was successfully updated");
    }
}
