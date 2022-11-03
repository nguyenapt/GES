using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class AutoCompleteModel
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string CompanyName { get; set; }
        public int SortOrder { get; set; }
        public int SortCategory { get; set; }
        public decimal? MarketCap { get; set; }
    }
}
