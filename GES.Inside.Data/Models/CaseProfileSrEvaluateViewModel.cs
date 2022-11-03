using GES.Inside.Data.Models.CaseProfiles;
using System;

namespace GES.Inside.Data.Models
{
    public class CaseProfileSrEvaluateViewModel : CaseProfileCoreViewModel
    {
        public DateTime StartingDate { get; set; }

        public string CaseRecommendation { get; set; }

        public string Guidelines { get; set; }

        //LogAndDialogReport

        public string SourceDialogSummary { get; set; }

        public string LatestNews { get; set; }

        //GesCommentary
        //Calendar
        //Additional Incidents
    }
}