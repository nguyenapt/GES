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
    
    public partial class I_GesCaseReportsI_Norms
    {
        public long I_GesCaseReportsI_Norms_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public long I_Norms_Id { get; set; }
    
        public virtual I_Norms I_Norms { get; set; }
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}
