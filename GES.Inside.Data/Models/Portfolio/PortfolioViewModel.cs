using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models.Portfolio
{
    public class PortfolioViewModel
    {
        public long PortfolioOrganizationId { get; set; }
        public long PortfolioId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long OrganizationId { get; set; }
        public bool IncludeInAlerts { get; set; }
        public bool ShowInCSC { get; set; }
        public bool StandardPortfolio { get; set; }
        public DateTime Created { get; set; }
        public Int32 Companies { get; set; }
        public Int32 ControversialActivities { get; set; }
        public Int32 GEServices { get; set; }
        public IEnumerable<long> GEServiceIds { get; set; }
        public Int32 Clients { get; set; }
        public bool GESStandardUniverse { get; set; }
    }
}
