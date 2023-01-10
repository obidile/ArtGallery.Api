using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities;

public class Payment: BaseObject
{
    public int Amount { get; set; }
}
