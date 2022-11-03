using System;

namespace GES.Inside.Data.Models
{
    public class OrganizationsServicesViewModel
    {
        public long OrganizationsServicesId { get; set; }
        public long OrganizationsId { get; set; }
        public long ServicesId { get; set; }
        public string ServicesName { get; set; }
        public long? ManagedDocumentsId { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
        public long? ModifiedByUsersId { get; set; }
        public DateTime? DemoEnd { get; set; }
        public long ServiceStatesId { get; set; }
        public bool TermsAccepted { get; set; }
        public string TermsAcceptedByIp { get; set; }
        public bool SuperFilter { get; set; }
        public decimal? Price { get; set; }
        public string Reporting { get; set; }
        public string Comment { get; set; }
        
        public string ModifiedString { get; set; }
    }
}