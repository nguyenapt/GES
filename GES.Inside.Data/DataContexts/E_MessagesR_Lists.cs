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
    
    public partial class E_MessagesR_Lists
    {
        public long E_MessagesR_Lists_Id { get; set; }
        public long E_Messages_Id { get; set; }
        public long R_Lists_Id { get; set; }
    
        public virtual E_Messages E_Messages { get; set; }
        public virtual R_Lists R_Lists { get; set; }
    }
}
