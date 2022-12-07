using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities;

public class Payment: BaseObject
{
    public int Amount { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public bool Status { get; set; }
}
