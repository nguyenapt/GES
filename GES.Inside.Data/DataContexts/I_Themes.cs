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
    
    public partial class I_Themes
    {
        public long I_Themes_Id { get; set; }
        public string Name { get; set; }
        public string Goal { get; set; }
        public string NextStep { get; set; }
        public string LatestNews { get; set; }
        public Nullable<long> ContactG_Users_Id { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual G_Users G_Users { get; set; }
    }
}
