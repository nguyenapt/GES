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
    
    public partial class I_RiskCompaniesStatistics
    {
        public long I_RiskCompaniesStatistics_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public Nullable<long> EnvGeneralRiskI_CharScores_Id { get; set; }
        public Nullable<long> PreparednessRatingI_CharScores_Id { get; set; }
        public Nullable<long> PerformanceRatingI_CharScores_Id { get; set; }
        public Nullable<long> TotalEnvRatingI_CharScores_Id { get; set; }
        public Nullable<long> HrGeneralRiskI_CharScores_Id { get; set; }
        public Nullable<long> EmployeeRatingI_CharScores_Id { get; set; }
        public Nullable<long> CommunityRatingI_CharScores_Id { get; set; }
        public Nullable<long> SupplierRatingI_CharScores_Id { get; set; }
        public Nullable<long> TotalHrRatingI_CharScores_Id { get; set; }
        public Nullable<long> BoardManagementRatingI_CharScores_Id { get; set; }
        public Nullable<long> TransparencyRatingI_CharScores_Id { get; set; }
        public Nullable<long> ShareholderRightsRatingI_CharScores_Id { get; set; }
        public Nullable<long> TotalCgRatingI_CharScores_Id { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual I_CharScores I_CharScores { get; set; }
        public virtual I_CharScores I_CharScores1 { get; set; }
        public virtual I_CharScores I_CharScores2 { get; set; }
        public virtual I_CharScores I_CharScores3 { get; set; }
        public virtual I_CharScores I_CharScores4 { get; set; }
        public virtual I_CharScores I_CharScores5 { get; set; }
        public virtual I_CharScores I_CharScores6 { get; set; }
        public virtual I_CharScores I_CharScores7 { get; set; }
        public virtual I_CharScores I_CharScores8 { get; set; }
        public virtual I_CharScores I_CharScores9 { get; set; }
        public virtual I_CharScores I_CharScores10 { get; set; }
        public virtual I_CharScores I_CharScores11 { get; set; }
        public virtual I_CharScores I_CharScores12 { get; set; }
        public virtual I_Companies I_Companies { get; set; }
    }
}
