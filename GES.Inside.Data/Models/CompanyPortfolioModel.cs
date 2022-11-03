using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class CompanyPortfolioModel
    {
        public long PortfolioId { get; set; }
        public string PortfolioName { get; set; }
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrgNr { get; set; }
        public string Phone { get; set; }
        public long PortfolioTypeId { get; set; }
        public DateTime? Created { get; set; }
    }
}