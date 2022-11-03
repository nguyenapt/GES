using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models.CaseProfiles
{
    public interface ICaseProfileCaseComponent
    {
        long CaseProfileId { get; set; }
        DateTime? AlertEntryDate { get; set; }
        string Heading { get; set; }
        string Norm { get; set; }
        long NormId { get; set; }
        string EngagementTheme { get; set; }
        string Location { get; set; }
        [Display(Name = "Engagement status")]
        string Recommendation { get; set; }
        ICollection<PublishedComponent> RecommendationArchive { get; set; }
        ICollection<PublishedComponent> ConclusionArchive { get; set; }
        string ConfirmedSymbol { get; set; }
        string CustomContent { get; set; }
        string CountryCode { get; set; }
        ICaseProfileStatusViewModel StatusComponent { get; set; }
        long? ConclusionId { get; set; }
        string Conclusion { get; set; }
        bool ConfirmedViolation { get; set; }
        bool IndicationOfViolation { get; set; }
    }
    public class CaseProfileCaseComponent : ICaseProfileCaseComponent
    {
        // Case
        
        [Display(Name = "Entry date")]
        public DateTime? AlertEntryDate { get; set; }

        [Display(Name = "Heading")]
        public string Heading { get; set; }

        [Display(Name = "Norm")]
        public string Norm { get; set; }
        
        [Display(Name = "Engagement type")]
        public string EngagementTheme { get; set; }

        public long NormId { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        public string CountryCode { get; set; }

        [Display(Name = "Engagement status")]
        public string Recommendation { get; set; }

        public ICollection<PublishedComponent> RecommendationArchive { get; set; }
        public ICollection<PublishedComponent> ConclusionArchive { get; set; }

        [Display(Name = "Confirmed")]
        public string ConfirmedSymbol { get; set; }

        public long CaseProfileId { get; set; }

        public string CustomContent { get; set; }
        public ICaseProfileStatusViewModel StatusComponent { get; set; }
        [Display(Name = "Conclusion")]
        public string Conclusion { get; set; }
        public long? ConclusionId { get; set; }
        [Display(Name = "Confirmed")]
        public bool ConfirmedViolation { get; set; }
        public bool IndicationOfViolation { get; set; }

        [Display(Name = "Confirmed date")]
        public string ConfirmedViolationDate { get; set; }
    }

    public class BcEngageCaseProfileCaseComponent : CaseProfileCaseComponent
    {
        
    }

    public class BcDisEngageCaseProfileCaseComponent : CaseProfileCaseComponent
    {
        
    }

    public class BcArchivedCaseProfileCaseComponent : CaseProfileCaseComponent
    {
       
    }

    public class BcEvaluateCaseProfileCaseComponent : CaseProfileCaseComponent
    {
        
    }

    public class FullAttributeCaseProfileCaseComponent : CaseProfileCaseComponent
    {
        [Display(Name = "Focus")]
        public string Theme { get; set; }

        [Display(Name = "Norm")]
        public string NormArea { get; set; }
    }

    public class SrCaseProfileCaseComponent : CaseProfileCaseComponent
    {
        [Display(Name = "Focus")]
        public string Theme { get; set; }

        [Display(Name = "Norm")]
        public string NormArea { get; set; }
    }
}
