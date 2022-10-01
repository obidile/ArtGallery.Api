using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Handlers.OrderItems.Commands;

public partial class UpdateOrderItemCommand : IRequest<ResponseModel>
{
    public long OrderItemId { get; set; }
    public long ProductId { get; set; }
    public long Quantity { get; set; }
    public long UnitPrice { get; set; }
    public long Discount { get; set; }
}

public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
{
    public UpdateOrderItemCommandValidator()
    {
        RuleFor(v => v.OrderItemId).NotEmpty();
    }
}

public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, ResponseModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateOrderItemCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResponseModel> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.Id == request.OrderItemId);

        if (orderItem == null)
        {
            return ResponseModel.Failure("OrderItem not found");
        }

        orderItem.ProductId = request.ProductId;
        orderItem.Quantity = request.Quantity;
        orderItem.UnitPrice = request.UnitPrice;
        orderItem.Discount = request.Discount;


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<OrderItemModel>.Success(_mapper.Map<OrderItemModel>(orderItem), "OrderItem was successfully updated");
    }
}
