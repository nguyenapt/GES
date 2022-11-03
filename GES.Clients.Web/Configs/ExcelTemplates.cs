using System.Configuration;

namespace GES.Clients.Web.Configs
{
    public static class ExcelTemplates
    {
        public static string ScreeningReport => "~/App_Data/Templates/Excel/Screening-Report.xlsx";
        public static string ScreeningReportPrefix => "Sustainalytics-Screening-Report-";
    }
}