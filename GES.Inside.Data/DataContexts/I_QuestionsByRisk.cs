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
    
    public partial class I_QuestionsByRisk
    {
        public long I_QuestionsByRisk_Id { get; set; }
        public long RiskI_CharScores_Id { get; set; }
        public long I_Questions_Id { get; set; }
    
        public virtual I_CharScores I_CharScores { get; set; }
        public virtual I_Questions I_Questions { get; set; }
    }
}