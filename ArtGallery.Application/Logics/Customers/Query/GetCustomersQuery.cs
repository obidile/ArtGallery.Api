using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArtGallery.Application.Logics.Customers.Query;

public class GetCustomersQuery : IRequest<List<CustomerModel>>
{
    public string fullName { get; set; }
    public string phoneNumber { get; set; }
    public string mailAddress { get; set; }
}

public class CustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public CustomersQueryHandler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<CustomerModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Customers.AsNoTracking();

        if (!string.IsNullOrEmpty(request.fullName))
        {
            query = query.Where(x => x.firstName == request.fullName || x.lastName == request.fullName);
        }

        if (!string.IsNullOrEmpty(request.mailAddress))
        {
            query = query.Where(x => x.mailAddress == request.mailAddress);
        }

        if (!string.IsNullOrEmpty(request.phoneNumber))
        {
            query = query.Where(x => x.phoneNumber == request.phoneNumber);
        }

        var result = await query.ProjectTo<CustomerModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}
