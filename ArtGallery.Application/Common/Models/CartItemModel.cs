using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models;

public class CartItemModel : BaseModel, IMapFrom<CartItem>
{
    public long CartId { get; set; }
    public Cart Carts { get; set; }
    public long ArtworkId  { get; set; }
    public ArtWork  ArtWork { get; set; }

}
