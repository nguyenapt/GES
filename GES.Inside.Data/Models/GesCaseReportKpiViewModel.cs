using System;

namespace GES.Inside.Data.Models
{
    public class GesCaseReportKpiViewModel
    {
        public long I_GesCaseReportsI_Kpis_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public long I_Kpis_Id { get; set; }
        public long I_KpiPerformance_Id { get; set; }
        public DateTime? Created { get; set; }
    }
}