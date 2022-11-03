using GES.Inside.Data.Models.CaseProfiles;
using System;

namespace GES.Inside.Data.Models
{
    public class CaseProfileSrEngageCropGovernance : CaseProfileCoreViewModel
    {
        public DateTime StartingDate { get; set; }

        public string Conclusion { get; set; }

        public string CaseRecommendation { get; set; }

        public string Description { get; set; }

        public string LatestNews { get; set; }

        public string Endorsement { get; set; }

        //Calendar
        //AdditionalIncidents

        public string ChangeObjective { get; set; }

        public string NextStep { get; set; }

        //status
    }
}