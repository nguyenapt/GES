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
    
    public partial class E_MessageContentG_Individuals
    {
        public long E_MessageContentG_Individuals_Id { get; set; }
        public long E_MessageContent_Id { get; set; }
        public long G_Individuals_Id { get; set; }
        public Nullable<System.DateTime> Sent { get; set; }
    
        public virtual E_MessageContent E_MessageContent { get; set; }
        public virtual G_Individuals G_Individuals { get; set; }
    }
}
