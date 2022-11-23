using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;
using ArtGallery.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Application.Common.Models
{
    public class OrderModel : BaseModel, IMapFrom<Order>
    {
        public OtherStatus Status { get; set; }
        public long OrderAmount { get; set; }
        public long AmountPaid { get; set; }
        public bool PaymentStatus { get; set; }
        public DateTime orderDate { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public User User { get; set; }
        public List<OrderItem> OrderItem { get; set; }
    }
}
