//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GES.Inside.Data.DataContexts
{
    using System;
    using System.Collections.Generic;
    
    public partial class GesUNGPAssessmentFormResources
    {
        public System.Guid GesUNGPAssessmentFormResourcesId { get; set; }
        public Nullable<System.Guid> GesUNGPAssessmentFormId { get; set; }
        public string SourcesName { get; set; }
        public string SourcesLink { get; set; }
        public Nullable<System.DateTime> SourceDate { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual GesUNGPAssessmentForm GesUNGPAssessmentForm { get; set; }
    }
}
