using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace ArtGallery.Application.Logics.OrderItems;

public class CreateOrderItemCommand : IRequest<string>
{
    public long Quantity { get; set; }
    public long UnitPrice { get; set; }
    public long Discount { get; set; }
    public long ArtworkId { get; set; }
    public ArtWork ArtWork { get; set; }
    public long orderId { get; set; }
    public Order Order { get; set; }
}
public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, string>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    public CreateOrderItemCommandHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var product = await _dbContext.OrderItems.AsNoTracking().FirstOrDefaultAsync(x => x.Id == request.orderId);
        if (product == null)
        {
            return "Order has not been placed";
        }

        var model = _mapper.Map<OrderItem>(request);

        _dbContext.OrderItems.Add(model);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return "OrderItem Successfully created";
    }
}
