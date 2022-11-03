using System;

namespace GES.Inside.Data.Models
{
    public class ServicesModel
    {
        public long GServicesId { get; set; }
        public int? Sort { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public bool ShowInNavigation { get; set; }
        public bool ShowInClient { get; set; }
        public string ReportLetter { get; set; }
        public long? EngagementTypesId { get; set; }
        public string EngagementTypesName { get; set; }
    }
}