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
    
    public partial class G_UsersG_SecurityGroups
    {
        public long G_UsersG_SecurityGroups_Id { get; set; }
        public long G_Users_Id { get; set; }
        public long G_SecurityGroups_Id { get; set; }
    
        public virtual G_SecurityGroups G_SecurityGroups { get; set; }
        public virtual G_Users G_Users { get; set; }
    }
}
