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
    
    public partial class I_MsciIndustryGroups
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_MsciIndustryGroups()
        {
            this.I_MsciIndustries = new HashSet<I_MsciIndustries>();
            this.I_Companies = new HashSet<I_Companies>();
            this.I_Companies1 = new HashSet<I_Companies>();
        }
    
        public long I_MsciIndustryGroups_Id { get; set; }
        public Nullable<long> I_MsciSectors_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_MsciIndustries> I_MsciIndustries { get; set; }
        public virtual I_MsciSectors I_MsciSectors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_Companies> I_Companies { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_Companies> I_Companies1 { get; set; }
    }
}
