using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class ScreeningReportInfoViewModel
    {
        public String name;

        public String sheetName() { return name.Substring(0, Math.Min(28, name.Length)); }
        
        public List<ExcelCaseProfile> cs = new List<ExcelCaseProfile>();
    }
}