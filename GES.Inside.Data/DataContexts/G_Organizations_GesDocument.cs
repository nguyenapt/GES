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
    
    public partial class G_Organizations_GesDocument
    {
        public long G_Organizations_GesDocument_Id { get; set; }
        public Nullable<long> G_Organizations_Id { get; set; }
        public Nullable<System.Guid> GesDocumentId { get; set; }
    
        public virtual GesDocument GesDocument { get; set; }
        public virtual G_Organizations G_Organizations { get; set; }
    }
}
