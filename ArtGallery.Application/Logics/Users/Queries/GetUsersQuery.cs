using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Users.Queries;

public class GetUsersQuery : IRequest<List<UserModel>>
{

}

public class UsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public UsersQueryHandler(IApplicationContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<List<UserModel>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Users.AsNoTracking().AsNoTracking().OrderBy(x => x.FirstName).ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}

