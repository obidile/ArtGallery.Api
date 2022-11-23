using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models;

public class CustomerModel : BaseModel, IMapFrom<Customer>
{
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string mailAddress { get; set; }
    public string phoneNumber { get; set; }
    public DateTime dateOfBirth { get; set; }
}