using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileBaseComponent
    {
        long CaseProfileId { get; set; }
        long CompanyId { get; set; }
        string CompanyName { get; set; }
        string CompanyIsin { get; set; }
        string CompanyIndustry { get; set; }
        string CompanyHomeCountry { get; set; }
        string CompanyHomeCountryCode { get; set; }
        string GicsSector { get; set; }
        string Website { get; set; }
        string MsciIndustry { get; set; }
        string Grade { get; set; }
        string PrimeThreshold { get; set; }
        int Dialogues { get; set; }
        int Meetings { get; set; }
        int ConferenceCalls { get; set; }
    }

    public class CaseProfileBaseComponent : ICaseProfileBaseComponent
    {
        // Base

        public long CompanyId { get; set; }

        [Display(Name = "Company")]
        public string CompanyName { get; set; }

        [Display(Name = "ISIN")]
        public string CompanyIsin { get; set; }

        [Display(Name = "SubIndustry")]
        public string CompanyIndustry { get; set; }

        [Display(Name = "Domicile")]
        public string CompanyHomeCountry { get; set; }

        [Display(Name = "Domicile Code")]
        public string CompanyHomeCountryCode { get; set; }

        public long CaseProfileId { get; set; }

        public string GicsSector { get; set; }
        public string Website { get; set; }
        public string MsciIndustry { get; set; }
        public string Grade { get; set; }
        public string PrimeThreshold { get; set; }
        public int Dialogues { get; set; }
        public int Meetings { get; set; }
        public int ConferenceCalls { get; set; }
    }

    public class SrEmeCaseProfileBaseComponent : CaseProfileBaseComponent
    {
        [Display(Name = "GRI aligned reporting")]
        public bool Gri { get; set; }

        [Display(Name = "UN Global Compact")]
        public bool GlobalCompactMember { get; set; }
    }

    public class FullAttributeCaseProfileBaseComponent : CaseProfileBaseComponent
    {
        [Display(Name = "GRI aligned reporting")]
        public bool Gri { get; set; }

        [Display(Name = "UN Global Compact")]
        public bool GlobalCompactMember { get; set; }
    }
}
