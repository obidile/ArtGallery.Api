using ArtGallery.Domain.Entities;
using ArtGallery.Application.Common.Mappers;

namespace ArtGallery.Application.Common.Models
{
    public class CartModel : BaseModel, IMapFrom<Cart>
    {
        public string CartSessionKey { get; set; }
        public long? UserId { get; set; }
        public long ArtWorkId { get; set; }
        public long Quantity { get; set; }
        public List<CartItem> CartItem { get; set; }
    }
}
