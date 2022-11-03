using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models
{
    public interface ICaseProfileCoreViewModel
    {
        long CaseProfileId { get; set; }
        ICaseProfileBaseComponent BaseComponent { get; set; }
        ICaseProfileCaseComponent CaseComponent { get; set; }
        ICaseProfileIssueComponent IssueComponent { get; set; }
        IEnumerable<DocumentViewModel> AdditionalDocuments { get; set; }
        GesContact ContactEngagementManager { get; set; }
        IEnumerable<RevisionViewModel> RevisionCriterials { get; set; }
        IEnumerable<ReferenceViewModel> References { get; set; }
        ConfirmationInformationViewModel ConfirmationInformation { get; set; }
        AlertViewModel Alert { get; set; }
        ICaseProfileUNGPAssessmentComponent CaseProfileUNGPAssessmentMethodologyComponent { get; set; }

        long? NewI_GesCaseReportStatuses_Id { get; set; }

    }

    public class CaseProfileCoreViewModel: ICaseProfileCoreViewModel
    {
        // Base
        public ICaseProfileBaseComponent BaseComponent { get; set; }
        
        // Case
        public ICaseProfileCaseComponent CaseComponent { get; set; }

        // Issue
        public ICaseProfileIssueComponent IssueComponent { get; set; }
        
        // Additional documents
        // List of documents: with other metadata: tags, desc, etc.
        public IEnumerable<DocumentViewModel> AdditionalDocuments { get; set; }
        public IEnumerable<RevisionViewModel> RevisionCriterials { get; set; }
        public IEnumerable<ReferenceViewModel> References { get; set; }

        // GES Contact Info
        [Display(Name = "Engagement Manager")]
        public GesContact ContactEngagementManager { get; set; }

        public long CaseProfileId { get; set; }
        public ConfirmationInformationViewModel ConfirmationInformation { get; set; }
        public AlertViewModel Alert { get; set; }

        public ICaseProfileUNGPAssessmentComponent CaseProfileUNGPAssessmentMethodologyComponent { get; set; }

        [Display(Name = "Discussion points")]
        public IList<I_EngagementDiscussionPoints> DiscussionPoints { get; set; }
        public long? NewI_GesCaseReportStatuses_Id { get; set; }
        public IList<GesCaseProfileTemplatesViewModel> CaseProfileInvisibleEntities { get; set; }
    }
}