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
    
    public partial class I_PortfolioCompaniesImport
    {
        public long I_PortfolioCompaniesImport_Id { get; set; }
        public Nullable<long> I_Portfolios_Id { get; set; }
        public string MsciName { get; set; }
        public string FtseName { get; set; }
        public string SixName { get; set; }
        public string OtherName { get; set; }
        public string Sedol { get; set; }
        public string Isin { get; set; }
        public string CountryIsoAlpha2 { get; set; }
        public string MsciCode { get; set; }
        public string FtseCode { get; set; }
        public Nullable<long> ListType { get; set; }
        public Nullable<long> MasterI_Companies_Id { get; set; }
        public string ListSource { get; set; }
        public bool Import { get; set; }
        public bool Screened { get; set; }
        public Nullable<long> SustainalyticsID { get; set; }
    
        public virtual I_CompanyListTypes I_CompanyListTypes { get; set; }
        public virtual I_Portfolios I_Portfolios { get; set; }
    }
}
