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
    
    public partial class I_NaArticles
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_NaArticles()
        {
            this.I_NasI_NaArticles = new HashSet<I_NasI_NaArticles>();
            this.I_GesCaseReports = new HashSet<I_GesCaseReports>();
        }
    
        public long I_NaArticles_Id { get; set; }
        public Nullable<long> I_Companies_Id { get; set; }
        public Nullable<long> I_NormAreas_Id { get; set; }
        public Nullable<long> G_Countries_Id { get; set; }
        public Nullable<long> I_NaTypes_Id { get; set; }
        public string Heading { get; set; }
        public string Text { get; set; }
        public string Source { get; set; }
        public Nullable<System.DateTime> SourceDate { get; set; }
        public string Source2 { get; set; }
        public Nullable<System.DateTime> SourceDate2 { get; set; }
        public string Source3 { get; set; }
        public Nullable<System.DateTime> SourceDate3 { get; set; }
        public string Source4 { get; set; }
        public Nullable<System.DateTime> SourceDate4 { get; set; }
        public bool Publishable { get; set; }
        public Nullable<long> SourceG_ManagedDocuments_Id { get; set; }
        public Nullable<long> Source2G_ManagedDocuments_Id { get; set; }
        public Nullable<long> Source3G_ManagedDocuments_Id { get; set; }
        public Nullable<long> Source4G_ManagedDocuments_Id { get; set; }
        public Nullable<long> RMI_Msci_Id { get; set; }
        public bool IsExtended { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<long> ModifiedByG_Users_Id { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.Guid> CountryId { get; set; }
    
        public virtual Countries Countries { get; set; }
        public virtual G_Users G_Users { get; set; }
        public virtual I_Companies I_Companies { get; set; }
        public virtual I_NormAreas I_NormAreas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_NasI_NaArticles> I_NasI_NaArticles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_GesCaseReports> I_GesCaseReports { get; set; }
    }
}