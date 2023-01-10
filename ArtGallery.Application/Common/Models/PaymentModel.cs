using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models;

public class PaymentModel : BaseModel, IMapFrom<Payment>
{
    public int Amount { get; set; }
}
