using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Handlers.OrderItems.Commands;

public partial class UpdateOrderItemCommand : IRequest<ResponseModel>
{
    public long Quantity { get; set; }
    public long UnitPrice { get; set; }
    public long Discount { get; set; }
    public long ArtworkId { get; set; }
    public ArtWork ArtWork { get; set; }
    public long orderId { get; set; }
    public Order Order { get; set; }
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
        var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(x => x.Id == request.orderId);

        if (orderItem == null)
        {
            return ResponseModel.Failure("OrderItem not found");
        }
        orderItem = _mapper.Map<OrderItem>(request);


        await _dbContext.SaveChangesAsync(cancellationToken);

        return ResponseModel<OrderItemModel>.Success(_mapper.Map<OrderItemModel>(orderItem), "OrderItem was successfully updated");
    }
}
