using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.ModelBinding;
using System.Web.Mvc;

namespace GES.Inside.Web.Models
{
    public class NormViewModel
    {
        public long I_GesCaseReportsI_Norms_Id { get; set; }
        public long I_GesCaseReports_Id { get; set; }
        public long I_Norms_Id { get; set; }

    }
}