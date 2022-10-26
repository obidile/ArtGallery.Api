using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.ArtWorks.Queries;

public class GetArtWorksQuery : IRequest<List<ArtWorkModel>>
{


}

public class ArtWorksQueryHandler : IRequestHandler<GetArtWorksQuery, List<ArtWorkModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public ArtWorksQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<ArtWorkModel>> Handle(GetArtWorksQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.ArtWorks.AsNoTracking().AsNoTracking().OrderBy(x => x.ArtWorkId).ProjectTo<ArtWorkModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}