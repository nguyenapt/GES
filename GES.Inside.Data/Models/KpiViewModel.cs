using System;

namespace GES.Inside.Data.Models
{
    public class KpiViewModel
    {
        public long KpiId { get; set; }
        public string KpiPerformance { get; set; }
        public string KpiName { get; set; }
        public string KpiDescription { get; set; }
        public DateTime Created { get; set; }
    }
}