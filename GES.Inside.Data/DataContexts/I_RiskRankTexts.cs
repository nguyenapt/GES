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
    
    public partial class I_RiskRankTexts
    {
        public long I_RiskRankTexts_Id { get; set; }
        public long I_Categories_Id { get; set; }
        public Nullable<decimal> PortfolioAvgRangeStart { get; set; }
        public Nullable<decimal> PortfolioAvgRangeEnd { get; set; }
        public Nullable<decimal> IndustryAvgRangeStart { get; set; }
        public Nullable<decimal> IndustryAvgRangeEnd { get; set; }
        public Nullable<int> PortfolioRankRangeStart { get; set; }
        public Nullable<int> PortfolioRankRangeEnd { get; set; }
        public Nullable<int> IndustryRankRangeStart { get; set; }
        public Nullable<int> IndustryRankRangeEnd { get; set; }
        public string Text { get; set; }
        public Nullable<int> SortOrder { get; set; }
    
        public virtual I_Categories I_Categories { get; set; }
    }
}
