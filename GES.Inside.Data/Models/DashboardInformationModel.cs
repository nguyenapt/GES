using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class DashboardInformationModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int NumberOfCompanies { get; set; }
        public int NumberOfCases { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
