using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Customers.Query;

public class GetCustomerByIdQuery : IRequest<CustomerModel>
{
	public GetCustomerByIdQuery(long Id)
	{
        this.Id = Id;
	}
    public long Id { get; set; }
}
public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerByIdQuery, CustomerModel>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

	public GetCustomerByIdQueryHandler(IApplicationContext dbContext, IMapper mapper)
	{
        _dbContext = dbContext;
        _mapper = mapper;
    }
    public async Task<CustomerModel> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
	{
        var data = await _dbContext.Customers.AsNoTracking().Where(x => x.Id == request.Id).ProjectTo<CustomerModel>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

        return data;
    }
}