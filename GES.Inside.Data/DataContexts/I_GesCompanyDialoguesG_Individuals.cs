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
    
    public partial class I_GesCompanyDialoguesG_Individuals
    {
        public long I_GesCompanyDialoguesG_Individuals_Id { get; set; }
        public long I_GesCompanyDialogues_Id { get; set; }
        public long G_Individuals_Id { get; set; }
    
        public virtual G_Individuals G_Individuals { get; set; }
        public virtual I_GesCompanyDialogues I_GesCompanyDialogues { get; set; }
    }
}
