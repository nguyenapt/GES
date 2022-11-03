using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models.CaseProfiles
{
   
    public class GesUngpAuditViewModel
    {
        
        public List<GesUngpAssessmentFormAuditViewModel> GesUngpAssessmentFormAuditViewModels { get; set; }
        public List<GesUngpAssessmentFormAuditViewModel> GesUngpAssessmentFormResourceAuditViewModels
        {
            get;
            set;
        }
    }
}