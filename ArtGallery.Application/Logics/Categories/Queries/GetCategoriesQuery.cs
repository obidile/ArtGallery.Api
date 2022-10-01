using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Categories.Queries;

public class GetCategoriesQuery : IRequest<List<CategoryModel>>
{

}

public class CategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CategoriesQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<CategoryModel>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Categories.AsNoTracking().AsNoTracking().OrderBy(x => x.Name).ProjectTo<CategoryModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}
