using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.OrderItems;

public class CreateOrderItemCommand : IRequest<ResponseModel>
{
    public long? UserId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public long UnitPrice { get; set; }
    public long Discount { get; set; }
}

public class CreateOrderItemCommandValidator : AbstractValidator<CreateOrderItemCommand>
{
    public CreateOrderItemCommandValidator()
    {
        RuleFor(x => x.UserId).Empty();
        RuleFor(x => x.ProductId).Empty();
        RuleFor(x => x.Quantity).Empty();
        RuleFor(x => x.UnitPrice).Empty();
        RuleFor(x => x.Discount).Empty();

    }
}

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateOrderItemCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var exist = await _dbContext.OrderItems.AsNoTracking().AnyAsync(x => x.ProductId == request.ProductId);
        if (exist)
        {
            return ResponseModel.Failure("Out of stock.");
        }

        var model = new OrderItem
        {
            UserId = request.UserId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Discount = request.Discount
        };

        _dbContext.OrderItems.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<OrderItemModel>.Success(_mapper.Map<OrderItemModel>(model), "OrderItem Successfully created");
    }
}
