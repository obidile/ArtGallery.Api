using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class Cart : BaseObject
    {
        public string CartSessionKey { get; set; }
        public long? UserId { get; set; }
        public long ArtWorkId { get; set; }
        public long Quantity { get; set; }
        public List<CartItem> CartItem { get; set; }
    }
}
