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
    
    public partial class I_GesCaseReportsExtra
    {
        public long I_GesCaseReportsExtra_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public string Keywords { get; set; }
        public Nullable<System.DateTime> EngagementSince { get; set; }
    
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}