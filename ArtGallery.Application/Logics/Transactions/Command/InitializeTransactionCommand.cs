using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using ArtGallery.Application.Logics.ArtWorks.Command;
using ArtGallery.Domain.Entities;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using PayStack.Net;

namespace ArtGallery.Application.Logics.Transactions.Command;
public class InitializeTransactionCommand : IRequest<string>
{
    public int Amount { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public bool Status { get; set; }
}
public class InitializeTransactionCommandHandler : IRequestHandler<InitializeTransactionCommand, string>
{
    private readonly IConfiguration _configuration;
    private readonly IApplicationContext _dbContext;
    private readonly IMapper _mapper;
    private readonly string token;
    private PayStackApi Paystack { get; set; }
    public InitializeTransactionCommandHandler(IConfiguration configuration, IApplicationContext dbContext, IMapper mapper)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _mapper = mapper;
        token = _configuration["Payment:PaystackSK"];
        Paystack = new PayStackApi(token);
    }
    public async Task<string> Handle(InitializeTransactionCommand request, CancellationToken cancellationToken)
    {
        var returnPath = _configuration.GetValue<string>("URL:callBack");
        TransactionInitializeRequest requester = new()
        {
            AmountInKobo = request.Amount * 100,
            Email = request.mailAddress,
            CallbackUrl = "returnPath"
        };

        TransactionInitializeResponse response = Paystack.Transactions.Initialize(requester);
        if (response.Status)
        {
            var model = new Payment()
            {
                Amount = request.Amount,
                mailAddress = request.mailAddress,
                firstName = request.firstName,
                lastName = request.lastName,
                Status = true,
            };
            _dbContext.Payments.Add(model);
            await _dbContext.SaveChangesAsync();
            return returnPath;
        }
        return "Payment error";
    }
}