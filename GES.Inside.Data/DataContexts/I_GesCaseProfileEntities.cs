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
    
    public partial class I_GesCaseProfileEntities
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_GesCaseProfileEntities()
        {
            this.GesCaseProfileInvisibleEntities = new HashSet<GesCaseProfileInvisibleEntities>();
        }
    
        public System.Guid GesCaseProfileEntity_Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public Nullable<int> Order { get; set; }
        public System.Guid I_GesCaseProfileEntitiesGroup_Id { get; set; }
        public Nullable<int> VisibleType { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GesCaseProfileInvisibleEntities> GesCaseProfileInvisibleEntities { get; set; }
        public virtual I_GesCaseProfileEntitiesGroup I_GesCaseProfileEntitiesGroup { get; set; }
    }
}
