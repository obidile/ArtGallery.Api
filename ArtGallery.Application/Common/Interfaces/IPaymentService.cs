using ArtGallery.Application.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Common.Interfaces
{
    public interface IPaymentService
    {
        Task<ResponseModel<PaymentModel>> ConfirmPayment(string referenceId);
    }
}
