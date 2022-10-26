using ArtGallery.Application.Common.Enums;
using ArtGallery.Application.Common.Mappers;
using ArtGallery.Domain.Entities;

namespace ArtGallery.Application.Common.Models
{
    public class UserModel : BaseModel, IMapFrom<User>
    {
        public AccountTypeEnum AccountType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string EmailAddress { get; set; }
        public bool IsActive { get; set; }
        public string EmailToken { get; set; }
        public bool EmailActivated { get; set; }
        public DateTime EmailActivatedDate { get; set; }
    }
}
