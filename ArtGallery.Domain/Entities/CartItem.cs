using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class CartItem : BaseObject
    {
        public long CartId { get; set; }
        public Cart Carts { get; set; }
        public long ArtworkId { get; set; }
        public ArtWork ArtWork { get; set; }
    }
}
