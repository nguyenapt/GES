using GES.Common.Configurations;

namespace GES.Inside.Data.Models
{
    public class GesContact
    {
        public long UserId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string JobTitle { get; set; }

        public string Email { get; set; }

        public string OrganizationName { get; set; }

        public long? OrganizationId { get; set; }

        public string Phone { get; set; }

        public string Comment { get; set; }

        //Organization
        public string Organization_Address { get; set; }
        
        public string Organization_PostalCode { get; set; }
        
        public string Organization_City { get; set; }

        public long? Organization_G_Countries_Id { get; set; }

        public string Organization_Phone { get; set; }

        public string Organization_WebsiteUrl { get; set; }

        public string Organization_Comment { get; set; }

        public bool Organization_Customer { get; set; }

        public int NumberCompanyDialogue { get; set; }

        public int NumberSourceDialogue { get; set; }
    }
}