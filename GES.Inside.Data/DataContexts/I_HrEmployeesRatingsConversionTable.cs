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
    
    public partial class I_HrEmployeesRatingsConversionTable
    {
        public long I_HrEmployeesRatingsConversionTable_Id { get; set; }
        public float RangeStart { get; set; }
        public float RangeEnd { get; set; }
        public long I_CharScores_Id { get; set; }
    
        public virtual I_CharScores I_CharScores { get; set; }
    }
}
