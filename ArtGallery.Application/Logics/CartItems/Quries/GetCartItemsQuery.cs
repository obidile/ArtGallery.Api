using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.CartItems.Quries;

public class GetCartItemsQuery : IRequest<List<CartItemModel>>
{
}
public class GetCartItemsQueryHandler : IRequestHandler<GetCartItemsQuery, List<CartItemModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public GetCartItemsQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }
    public async Task<List<CartItemModel>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.CartItems.AsNoTracking().AsNoTracking().OrderBy(x => x.ArtWork).ProjectTo<CartItemModel>
            (_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}