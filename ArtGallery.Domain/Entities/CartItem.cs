using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtGallery.Domain.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public long CartId { get; set; }
        public Cart Carts { get; set; }
        public long ArtworkId { get; set; }
        public ArtWork ArtWork { get; set; }
    }
}
