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
    
    public partial class I_FtseSectors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_FtseSectors()
        {
            this.I_FtseSubSectors = new HashSet<I_FtseSubSectors>();
            this.I_Companies = new HashSet<I_Companies>();
        }
    
        public long I_FtseSectors_Id { get; set; }
        public Nullable<long> I_FtseGroups_Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    
        public virtual I_FtseGroups I_FtseGroups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_FtseSubSectors> I_FtseSubSectors { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_Companies> I_Companies { get; set; }
    }
}
