using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Models
{
    public class CompanyManagementSystemModel
    {
        public long I_CompaniesI_ManagementSystems_Id { get; set; }
        public long I_Companies_Id { get; set; }
        public long I_ManagementSystems_Id { get; set; }
        public string ManagementSystemsName { get; set; }
        public bool Certification { get; set; }
        public Nullable<int> Coverage { get; set; }
        public Nullable<System.DateTime> Modified { get; set; }
        public Nullable<long> ModifiedByG_Users_Id { get; set; }
        public System.DateTime Created { get; set; }
    }
}
