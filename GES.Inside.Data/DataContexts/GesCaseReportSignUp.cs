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
    
    public partial class GesCaseReportSignUp
    {
        public long GesCaseReportSignUpId { get; set; }
        public Nullable<long> I_GesCaseReports_Id { get; set; }
        public Nullable<long> G_Individuals_Id { get; set; }
        public bool Active { get; set; }
        public Nullable<long> G_Organizations_Id { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
    
        public virtual G_Individuals G_Individuals { get; set; }
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}
