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
    
    public partial class I_EngagementIssuesInfo
    {
        public long I_EngagementIssuesInfo_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual I_Companies I_Companies { get; set; }
    }
}
