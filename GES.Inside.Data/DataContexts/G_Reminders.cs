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
    
    public partial class G_Reminders
    {
        public long G_Reminders_Id { get; set; }
        public long G_Bills_Id { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual G_Bills G_Bills { get; set; }
    }
}