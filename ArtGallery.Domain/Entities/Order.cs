using ArtGallery.Domain.Common;
using ArtGallery.Domain.Enums;

namespace ArtGallery.Domain.Entities
{
    public class Order : BaseObject
    {
        public StatusEnum Status { get; set; }
        public long OrderAmount { get; set; }
        public long AmountPaid { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime PaymentDate { get; set; }

        public User User { get; set; }
        public List<OrderItem> OrderItem { get; set; }
    }
}
