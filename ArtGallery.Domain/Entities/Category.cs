using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities
{
    public class Category : BaseObject
    {
        public long CategoryId { get; set; }
        public string Name { get; set; }


        public List<ArtWork> ArtWorks { get; set; }
    }
}
