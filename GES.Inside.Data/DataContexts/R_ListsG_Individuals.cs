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
    
    public partial class R_ListsG_Individuals
    {
        public long R_ListsG_Individuals_Id { get; set; }
        public long G_Individuals_Id { get; set; }
        public long R_Lists_Id { get; set; }
    
        public virtual G_Individuals G_Individuals { get; set; }
        public virtual R_Lists R_Lists { get; set; }
    }
}
