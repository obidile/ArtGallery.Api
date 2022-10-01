using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class Cart : BaseObject
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public long Discount { get; set; }

        public User User { get; set; }
    }
}
