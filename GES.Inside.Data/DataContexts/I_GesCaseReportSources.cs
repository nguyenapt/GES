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
    
    public partial class I_GesCaseReportSources
    {
        public long I_GesCaseReportSources_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public Nullable<long> G_ManagedDocuments_Id { get; set; }
        public bool MainSource { get; set; }
        public string Source { get; set; }
        public Nullable<short> PublicationYear { get; set; }
        public Nullable<long> I_GesCaseReportAvailabilityStatuses_Id { get; set; }
        public string AvailableFrom { get; set; }
        public Nullable<System.DateTime> Accessed { get; set; }
        public bool ShowInReport { get; set; }
    
        public virtual G_ManagedDocuments G_ManagedDocuments { get; set; }
        public virtual I_GesCaseReportAvailabilityStatuses I_GesCaseReportAvailabilityStatuses { get; set; }
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}
