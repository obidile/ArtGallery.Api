using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.Carts;

public class CreateCartCommand : IRequest<ResponseModel>
{
    public long UserId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public long Discount { get; set; }
}

public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
{
    public CreateCartCommandValidator()
    {
        RuleFor(x => x.UserId).Empty();
        RuleFor(v => v.ProductId).NotEmpty();
        RuleFor(v => v.Quantity).NotEmpty();
        RuleFor(x => x.Discount).Empty();
    }
}

public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateCartCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.Carts.AsNoTracking().AnyAsync(x => x.ProductId == request.ProductId);
        if (exist)
        {
            return ResponseModel.Failure("This artwork is on cart already.");
        }

        var model = new Cart
        {
            UserId = request.UserId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Discount = request.Discount,
        };

        _dbContext.Carts.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<CartModel>.Success(_mapper.Map<CartModel>(model), "ArtWork Was successfully added to Cart");
    }
}
