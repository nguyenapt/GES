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
    
    public partial class I_GesCaseProfileEntitiesGroup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_GesCaseProfileEntitiesGroup()
        {
            this.I_GesCaseProfileEntities = new HashSet<I_GesCaseProfileEntities>();
        }
    
        public System.Guid I_GesCaseProfileEntitiesGroup_Id { get; set; }
        public string Name { get; set; }
        public string GroupType { get; set; }
        public Nullable<int> Order { get; set; }
        public Nullable<int> VisibleType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCaseProfileEntities> I_GesCaseProfileEntities { get; set; }
    }
}
