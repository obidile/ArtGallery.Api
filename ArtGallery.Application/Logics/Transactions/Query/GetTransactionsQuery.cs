using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtGallery.Application.Logics.Transactions.Query;

public class GetTransactionsQuery : IRequest<List<PaymentModel>> //: IRequest<List<CustomerModel>>
{
    public int Amount { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public bool Status { get; set; }
}
public class GetTransactionsQueryHAndler : IRequestHandler<GetTransactionsQuery, List<PaymentModel>>
{
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;

    public GetTransactionsQueryHAndler(IApplicationContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<PaymentModel>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = _dbContext.Payments.AsNoTracking().Where(x => x.Status == true);
        var result = await transactions.ProjectTo<PaymentModel>(_mapper.ConfigurationProvider).ToListAsync();

        return result;
    }
}