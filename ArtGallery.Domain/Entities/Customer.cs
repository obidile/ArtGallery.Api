using ArtGallery.Domain.Common;

namespace ArtGallery.Domain.Entities;

public class Customer : BaseObject
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public string phoneNumber { get; set; }
    public DateTime dateOfBirth { get; set; }
}
