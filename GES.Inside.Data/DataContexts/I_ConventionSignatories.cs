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
    
    public partial class I_ConventionSignatories
    {
        public long I_ConventionSignatories1 { get; set; }
        public long I_Conventions_Id { get; set; }
        public Nullable<long> G_Countries_Id { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        public virtual I_Conventions I_Conventions { get; set; }
    }
}
