using ArtGallery.Domain.Common;
using ArtGallery.Domain.Enums;

namespace ArtGallery.Domain.Entities
{
    public class Order : BaseObject
    {
        public long UserId { get; set; }
        public OtherStatus Status { get; set; }
        public long OrderAmount { get; set; }
        public long AmountPaid { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; }
    }
}
