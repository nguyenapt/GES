using GES.Inside.Data.Models.CaseProfiles;
using System;

namespace GES.Inside.Data.Models
{
    public class CaseProfileStandardCoreViewModel : CaseProfileCoreViewModel
    {
        public DateTime AlertEntryDate { get; set; }

        public string CaseConclusion { get; set; }

        public string Guidelines { get; set; }

        public string SrourceDialogueSummary { get; set; }

        public string CompanyPrepareness { get; set; }

        public string LatestNews { get; set; }

        public string IssueConclusion { get; set; }

    }
}