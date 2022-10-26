using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models
{
    public class OrderItemModel : BaseModel, IMapFrom<OrderItem>
    {
        public long? OrderItemId { get; set; }
        public long ProductId { get; set; }
        public long Quantity { get; set; }
        public long UnitPrice { get; set; }
        public long Discount { get; set; }
    }
}
