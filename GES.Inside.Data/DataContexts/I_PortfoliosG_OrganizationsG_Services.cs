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
    
    public partial class I_PortfoliosG_OrganizationsG_Services
    {
        public long I_PortfoliosG_OrganizationsG_Services_Id { get; set; }
        public long I_PortfoliosG_Organizations_Id { get; set; }
        public long G_Services_Id { get; set; }
        public System.DateTime Created { get; set; }
    
        public virtual G_Services G_Services { get; set; }
        public virtual I_PortfoliosG_Organizations I_PortfoliosG_Organizations { get; set; }
    }
}
