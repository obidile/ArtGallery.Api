using ArtGallery.Application.Common.Models;

namespace ArtGallery.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<ResponseModel<PaymentModel>> ConfirmPayment(string reference);
}
