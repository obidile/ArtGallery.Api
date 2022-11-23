using ArtGallery.Application.Common.Interfaces;
using ArtGallery.Application.Common.Models;
using Microsoft.Extensions.Configuration;
using PayStack.Net;

namespace ArtGallery.Infrastucture.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        public PaymentService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<ResponseModel<PaymentModel>> ConfirmPayment(string referenceId)
        {
            var testOrLiveSecret = _configuration.GetValue<string>("PayStack:SecretKey");
            var api = new PayStackApi(testOrLiveSecret);
            var verifyResponse = api.Transactions.Verify(referenceId);
            var result = new ResponseModel<PaymentModel>()
            {
                Status = verifyResponse.Status,
                Message = verifyResponse.Message,
                Data = new PaymentModel()
                {
                    Amount = verifyResponse.Data.Amount
                }
            };
            return await Task.FromResult(result);
        }
    }
}
