using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class CaseReportSupplementaryViewModel
    {
        public long Id { get; set; }
        public long CaseReportId { get; set; }
        public long? ManagedDocumentId { get; set; }        
        public bool ShowInReport { get; set; }
        public string Source { get; set; }
        public int? PublicationYear { get; set; }
        public long? Status { get; set; }
        public string AvailableFrom { get; set; }
        public DateTime? Accessed { get; set; }
        public string Name { get; set; }
    }
}
