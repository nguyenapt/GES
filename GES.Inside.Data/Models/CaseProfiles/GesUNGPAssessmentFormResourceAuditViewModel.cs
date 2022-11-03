using System;
using System.Collections.Generic;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Models.CaseProfiles
{


    public class GesUngpAssessmentFormResourceAuditViewModel
    {
        public Guid GesUngpAssessmentFormResourceAuditId { get; set; }
        public Guid GesUngpAssessmentFormId { get; set; }
        public Guid GesUngpAssessmentFormResourcesId { get; set; }
        public string AuditDmlAction { get; set; }
        public string AuditUser { get; set; }
        public DateTime? AuditDatetime { get; set; }
        public IList<GesUngpAssessmentFormResourceAuditDetailsViewModel> GesUngpAssessmentFormResourceAuditDetailsViewModels
        {
            get;
            set;
        }
    }
}