using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Orders.Queries;

public class GetOrdersQuery : IRequest<List<OrderModel>>
{

}

public class OrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<OrderModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public OrdersQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<OrderModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Orders.AsNoTracking().AsNoTracking().OrderBy(x => x.OrderId).ProjectTo<OrderModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}
