using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class OrderItem : BaseObject
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
