using System;
using GES.Common.Configurations;

namespace GES.Inside.Web.Models
{
    public class GSSLinkViewModel
    {
        public Guid I_GSSLink_Id { get; set; }
        public long? I_GesCaseReports_Id { get; set; }
        public string Description { get; set; }
        public DateTime? GSSLinkModified { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedString => Created?.ToString(Configurations.DateWithTimeFormat);
        public string GSSLinkModifiedString => GSSLinkModified?.ToString(Configurations.DateWithTimeFormat);
    }
}