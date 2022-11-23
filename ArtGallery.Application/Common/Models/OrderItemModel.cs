using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models
{
    public class OrderItemModel : BaseModel, IMapFrom<OrderItem>
    {
        public long Quantity { get; set; }
        public long UnitPrice { get; set; }
        public long Discount { get; set; }
        public long ArtworkId { get; set; }
        public ArtWork ArtWork { get; set; }
        public long orderId { get; set; }
        public Order Order { get; set; }

        public User User { get; set; }
    }
}
