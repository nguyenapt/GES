using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class AssociatedCorporationsViewModel
    {
        public long AssociatedCorporationId { get; set; }
        public long? CaseReportId { get; set; }
        public string Name { get; set; }
        public string Ownership { get; set; }
        public string Comment { get; set; }
        public bool Traded { get; set; }
        public bool ShowInReport { get; set; }
    }
}
