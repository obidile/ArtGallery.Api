using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models
{
    public class ArtWorkModel : BaseModel, IMapFrom<ArtWork>
    {
        public long? ArtworkId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public long Rating { get; set; }
        public long Quantity { get; set; }
        public long Price { get; set; }
        public string Category { get; set; }
        public string ArtImage { get; set; }
        public long DisCount { get; set; }
        public DateTime ProductionYear { get; set; }


        public long CategoryId { get; set; }
        public Category Categories { get; set; }
    }
}
