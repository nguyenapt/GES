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
    
    public partial class I_GesCaseReportsI_Kpis_Audit
    {
        public Nullable<long> I_GesCaseReportsI_Kpis_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public long I_Kpis_Id { get; set; }
        public Nullable<long> I_KpiPerformance_Id { get; set; }
        public System.DateTime Created { get; set; }
        public string AuditDataState { get; set; }
        public string AuditDMLAction { get; set; }
        public string AuditUser { get; set; }
        public Nullable<System.DateTime> AuditDateTime { get; set; }
    }
}