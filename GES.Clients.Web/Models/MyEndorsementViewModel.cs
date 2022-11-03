using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using GES.Inside.Data.DataContexts;

namespace GES.Clients.Web.Models
{
    public class MyEndorsementViewModel
    {
        public long ActivityFormId { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public long? EngagementActivityOption { get; set; }

        public IEnumerable<SelectListItem> EngagementActivityOptions { get; set; }

        public bool Ownership { get; set; }

        public string NumberofShareCount { get; set; }

        public string NumberofShareDate { get; set; }

        public bool HoldingsThroughExternalFunds { get; set; }

        public bool FixedIncomeHoldings { get; set; }

        public bool SignLetters { get; set; }

        public bool ParticipateInConferenceCalls { get; set; }

        public bool ParticipateInLiveMeetings { get; set; }

        public bool FileorCoFileResolutions { get; set; }

        public bool CleaningHouseActions { get; set; }

        public bool QuestionToAgms { get; set; }

        public bool CollaborativeActions { get; set; }

        public string Suggestions { get; set; }

        public long GesCaseReportId { get; set; }

        public long OrgId { get; set; }
    }
}