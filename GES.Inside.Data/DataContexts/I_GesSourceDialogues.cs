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
    
    public partial class I_GesSourceDialogues
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_GesSourceDialogues()
        {
            this.I_GesSourceDialoguesG_Individuals = new HashSet<I_GesSourceDialoguesG_Individuals>();
        }
    
        public long I_GesSourceDialogues_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public Nullable<long> G_Individuals_Id { get; set; }
        public Nullable<System.DateTime> ContactDate { get; set; }
        public Nullable<long> I_ContactTypes_Id { get; set; }
        public string Action { get; set; }
        public string Notes { get; set; }
        public string Text { get; set; }
        public Nullable<long> I_ContactDirections_Id { get; set; }
        public Nullable<long> G_ManagedDocuments_Id { get; set; }
        public bool ShowInCsc { get; set; }
        public bool ShowInReport { get; set; }
        public bool ClassA { get; set; }
    
        public virtual G_Individuals G_Individuals { get; set; }
        public virtual I_ContactDirections I_ContactDirections { get; set; }
        public virtual I_ContactTypes I_ContactTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesSourceDialoguesG_Individuals> I_GesSourceDialoguesG_Individuals { get; set; }
        public virtual I_GesCaseReports I_GesCaseReports { get; set; }
    }
}
