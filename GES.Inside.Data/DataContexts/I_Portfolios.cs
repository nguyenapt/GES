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
    
    public partial class I_Portfolios
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public I_Portfolios()
        {
            this.I_PortfoliosG_Organizations = new HashSet<I_PortfoliosG_Organizations>();
            this.O_Sources = new HashSet<O_Sources>();
            this.I_PortfolioCompaniesImport = new HashSet<I_PortfolioCompaniesImport>();
        }
    
        public long I_Portfolios_Id { get; set; }
        public long G_Organizations_Id { get; set; }
        public string Name { get; set; }
        public bool ShowInCSC { get; set; }
        public bool StandardPortfolio { get; set; }
        public System.DateTime Created { get; set; }
        public long I_PortfolioTypes_Id { get; set; }
        public bool GESStandardUniverse { get; set; }
        public Nullable<System.DateTime> LastUpdated { get; set; }
    
        public virtual I_PortfolioTypes I_PortfolioTypes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_PortfoliosG_Organizations> I_PortfoliosG_Organizations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<O_Sources> O_Sources { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<I_PortfolioCompaniesImport> I_PortfolioCompaniesImport { get; set; }
    }
}
