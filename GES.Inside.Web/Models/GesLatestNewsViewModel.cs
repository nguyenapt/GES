using System;
using GES.Common.Configurations;

namespace GES.Inside.Web.Models
{
    public class GesLatestNewsViewModel
    {
        public long I_GesLatestNews_Id { get; set; }
        public long? I_GesCaseReports_Id { get; set; }
        public string Description { get; set; }
        public DateTime? LatestNewsModified { get; set; }
        public DateTime? Created { get; set; }
        public string CreatedString => Created?.ToString(Configurations.DateWithTimeFormat);
        public string LatestNewsModifiedString => LatestNewsModified?.ToString(Configurations.DateFormat);
    }
}