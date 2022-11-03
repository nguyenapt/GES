using System;

namespace GES.Inside.Data.Models
{
    public class GesCaseAuditLogsViewModel
    {
        public Guid Id { get; set; }
        public long I_GesCaseReportsId { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime AuditDatetime { get; set; }
        public string OldValueName { get; set; }
        public string NewValueName { get; set; }
    }
}