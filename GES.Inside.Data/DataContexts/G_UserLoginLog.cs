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
    
    public partial class G_UserLoginLog
    {
        public long G_UserLoginLog_Id { get; set; }
        public Nullable<long> G_Users_Id { get; set; }
        public string Ip { get; set; }
        public string Hostname { get; set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }
        public System.DateTime Created { get; set; }
    }
}
