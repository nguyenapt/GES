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
    
    public partial class Sdg
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sdg()
        {
            this.GesCaseReportSdg = new HashSet<GesCaseReportSdg>();
        }
    
        public long Sdg_Id { get; set; }
        public string Sdg_Name { get; set; }
        public string Sdg_Link { get; set; }
        public Nullable<System.Guid> DocumentId { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GesCaseReportSdg> GesCaseReportSdg { get; set; }
        public virtual GesDocument GesDocument { get; set; }
    }
}