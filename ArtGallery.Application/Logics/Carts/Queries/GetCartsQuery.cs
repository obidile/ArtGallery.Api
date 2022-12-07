using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Carts.Queries;

public class GetCartsQuery : IRequest<List<CartModel>>
{

}

public class CartsQueryHandler : IRequestHandler<GetCartsQuery, List<CartModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CartsQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<CartModel>> Handle(GetCartsQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Carts.AsNoTracking().OrderBy(x => x.CartSessionKey).ProjectTo<CartModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}