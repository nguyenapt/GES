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
    
    public partial class I_ControversialCompanies
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_ControversialCompanies()
        {
            this.I_ControversialComments = new HashSet<I_ControversialComments>();
            this.I_ControversialCompaniesI_ControversialActivites = new HashSet<I_ControversialCompaniesI_ControversialActivites>();
        }
    
        public long I_ControversialCompanies_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public string ContactFullName { get; set; }
        public string ContactTitle { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string History { get; set; }
        public string Observation { get; set; }
        public string Comment { get; set; }
        public Nullable<long> AnalystG_Users_Id { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<long> ModifiedByG_Users_Id { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual G_Users G_Users { get; set; }
        public virtual G_Users G_Users1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_ControversialComments> I_ControversialComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_ControversialCompaniesI_ControversialActivites> I_ControversialCompaniesI_ControversialActivites { get; set; }
    }
}