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
    
    public partial class T_Projects
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public T_Projects()
        {
            this.T_Activites = new HashSet<T_Activites>();
        }
    
        public long T_Projects_Id { get; set; }
        public string Name { get; set; }
        public Nullable<long> CustomerG_Organizations_Id { get; set; }
        public Nullable<int> EstimatedHours { get; set; }
        public Nullable<int> FreeHours { get; set; }
        public Nullable<decimal> HourlyRate { get; set; }
        public bool Closed { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public System.DateTime Modified { get; set; }
        public Nullable<long> ModifiedByG_Users_Id { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<long> G_Departments_Id { get; set; }
    
        public virtual G_Users G_Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_Activites> T_Activites { get; set; }
        public virtual G_Organizations G_Organizations { get; set; }
    }
}