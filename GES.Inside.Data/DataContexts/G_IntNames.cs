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
    
    public partial class G_IntNames
    {
        public long G_IntNames_Id { get; set; }
        public long G_IntNameIds_Id { get; set; }
        public long G_Languages_Id { get; set; }
        public string Name { get; set; }
    
        public virtual G_IntNameIds G_IntNameIds { get; set; }
        public virtual G_Languages G_Languages { get; set; }
    }
}
