using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Domain.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data.SqlTypes;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArtGallery.Application.Logics.ArtWorks.Queries;

public class GetArtWorksQuery : IRequest<List<ArtWorkModel>>
{
    public string title { get; set; }
    public string price { get; set; }

}

public class ArtWorksQueryHandler : IRequestHandler<GetArtWorksQuery, List<ArtWorkModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ArtWorksQueryHandler(IApplicationContext context, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = context;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<List<ArtWorkModel>> Handle(GetArtWorksQuery request, CancellationToken cancellationToken)
    {
        var check = _dbContext.ArtWorks.AsNoTracking();

        if (!string.IsNullOrEmpty(request.title))
        {
            check = check.Where(x => x.Title == request.title);
        }
        if (!string.IsNullOrEmpty(request.price))
        {
            check = check.Where(x => x.Price.ToString() == request.price);
        }

        var result = await check.ProjectTo<ArtWorkModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}