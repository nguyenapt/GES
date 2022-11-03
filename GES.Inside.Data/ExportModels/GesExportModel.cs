using GES.Inside.Data.Models;
using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.ExportModels
{
    public class GesExportModel<TData> where TData : class
    {
         public TData Data { get; set; }

        // Add more properties for export feature here...
        public bool ShowCoverPage { get; set; } = true;
        public bool ShowCompanyInfo { get; set; } = true;
        public bool ShowCaseInfoBusinessConduct { get; set; } = true;
        public bool ShowStatistic { get; set; } = true;
        public bool ShowCompanyEvents { get; set; } = true;
        public bool ShowSummary { get; set; } = true;
        public bool ShowAlerts { get; set; }
        public bool ShowDescription { get; set; }
        public bool ShowConclusion { get; set; }
        public bool ShowGesCommentary { get; set; }                
        public bool ShowLatestNews { get; set; }
        public bool ShowEngagementInformation { get; set; }
        public bool ShowDiscussionPoint { get; set; }
        public bool ShowOtherStakeholder { get; set; }
        public bool ShowKPI { get; set; }
        public bool ShowGuidelinesAndConventions { get; set; }
        public bool ShowConfirmationDetails { get; set; }
        public bool ShowReferences { get; set; }
        public bool ShowCompanyDialogue { get; set; }
        public bool ShowSourceDialogue { get; set; }                
        public bool ShowCompanyRelatedItems { get; set; }
        public bool ShowAdditionalDocuments { get; set; } = true;
        public bool ShowGesContactInformation { get; set; } = true;

        public bool ShowSummaryMaterialRisk { get; set; }

        public bool ShowClosingDetail { get; set; }

        public List<CaseReportListViewModel> AdditionalIncidents { get; set; }

        public IList<AlertListViewModel> Alerts { get; set; }

        public List<CaseProfileExportSDGViewModel> SDGImages { get; set; }

        public IEnumerable<EventListViewModel> UpcommingEvents { get; set; }
    }
}
