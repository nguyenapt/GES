using System;
using System.Collections.Generic;
using GES.Common.Enumeration;

namespace GES.Inside.Data.Models
{
    public class SidebarModel
    {
        public string Action { get; set; }
        public string Controller { get; set; }
        public string QueryString { get; set; }

        public bool AlertService { get; set; }
        public bool GlobalEthical { get; set; }
        public bool GesControversial { get; set; }
        public bool BusinessConduct { get; set; }
        public bool Burnma { get; set; }
        public bool CarbonRisk { get; set; }
        public bool EmerginMarkets { get; set; }
        public bool PalmOil { get; set; }
        public bool Taxation { get; set; }
        public bool Water { get; set; }
        public bool Governance { get; set; }
        public ClientType ClientType { get; set; }
        
        public IEnumerable<EngagementTypeCategoryView> EngagementTypeCategoryViews { get; set; }
    }
}
