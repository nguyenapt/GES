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
    
    public partial class L_UsersUsers
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public int UserId2 { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual G_Users G_Users { get; set; }
    }
}
