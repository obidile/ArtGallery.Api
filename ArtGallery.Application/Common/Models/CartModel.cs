using ArtGallery.Domain.Entities;
using ArtGallery.Application.Common.Mappers;

namespace ArtGallery.Application.Common.Models
{
    public class CartModel : BaseModel, IMapFrom<Cart>
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public long Discount { get; set; }

        public User User { get; set; }
    }
}
