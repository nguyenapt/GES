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
    
    public partial class GesCaseReportSdg
    {
        public long GesCaseReport_Sdg_Id { get; set; }
        public long GesCaseReport_Id { get; set; }
        public long Sdg_Id { get; set; }
        public Nullable<int> SortOrder { get; set; }
    
        public virtual Sdg Sdg { get; set; }
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}
