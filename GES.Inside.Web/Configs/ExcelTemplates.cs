using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Inside.Web.Configs
{
    public static class ExcelTemplates
    {
        public static string GesStandardUniverse => "~/App_Data/Templates/Excel/GesStandardUniverse.xlsx";
        public static string FullGesStandardUniverse => "~/App_Data/Templates/Excel/FullGesStandardUniverse.xlsx";
        public static string GesStandardUniversePrefix => "GesStandardUniverse_";

        public static string Endorsement => "~/App_Data/Templates/Excel/Endorsement.xlsx";
        public static string EndorsementPrefix => "Endorsement_";
         
        public static string ScreeningReport => "~/App_Data/Templates/Excel/StandardScreeningReport.xlsx";
        public static string ScreeningReportPrefix => "Sustainalytics-Screening-Report-";        
            
        
        public static string GlobalEthicalStandardScreeningReport => "~/App_Data/Templates/Excel/GlobalEthicalStandardScreeningReport.xlsx";
        public static string GlobalEthicalStandardScreeningReportPrefix => "Sustainalytics-Global-Ethical-Standard-Screening-Report-";

        public static string EFStatusReport => "~/App_Data/Templates/Excel/EFStatusReport.xlsx";
        public static string EFStatusReportPrefix => "EF-Status-Report-";

    }
}