using ArtGallery.Domain.Common;
using ArtGallery.Domain.Enums;

namespace ArtGallery.Domain.Entities
{
    public class User : BaseObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Location { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public AccountTypeEnum AccountType { get; set; }
        public string EmailToken { get; set; }
        public bool EmailActivated { get; set; }
        public DateTime? EmailActivatedDate { get; set; }
    }
}
