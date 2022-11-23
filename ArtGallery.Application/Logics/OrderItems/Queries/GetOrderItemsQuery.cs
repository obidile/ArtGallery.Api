using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.OrderItems.Queries;

public class GetOrderItemsQuery : IRequest<List<OrderItemModel>>
{

}

public class OrderItemsQueryHandler : IRequestHandler<GetOrderItemsQuery, List<OrderItemModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public OrderItemsQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<OrderItemModel>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.OrderItems.AsNoTracking().AsNoTracking().OrderBy(x => x.orderId).ProjectTo<OrderItemModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}
