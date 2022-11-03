using System;
using System.Collections.Generic;
using GES.Common.Configurations;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Models.CaseProfiles
{
   
    public class GesUngpAssessmentFormAuditViewModel
    {
        public Guid GesUngpAssessmentFormAuditId { get; set; }
        public Guid GesUngpAssessmentFormId { get; set; }
        public long GesCaseReportsId { get; set; }
        public string AuditDmlAction { get; set; }
        public string AudiAction
        {
            get
            {switch (AuditDmlAction)
                    {
                        case Constants.Insert: return "created";
                        case Constants.Update :return "made changes";
                        case Constants.Delete: return "deleted";
                        default: return "created";
                        
                    }
            }
        }
        public string AuditUser { get; set; }
        public DateTime? AuditDatetime { get; set; }
        public string AuditDatetimeString => AuditDatetime?.ToString(Configurations.DateWithTimeFormat);

        public List<GesUngpAssessmentFormAuditDetailsViewModel> GesUngpAssessmentFormAuditDetailsViewModels
        {
            get;
            set;
        }
    }
}