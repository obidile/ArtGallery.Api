using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class ArtWork : BaseObject
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public long Rating { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public string ArtImage { get; set; }
        public long? DisCount { get; set; }
        public DateTime ProductionYear { get; set; }

        public User User { get; set; }
    }
}
