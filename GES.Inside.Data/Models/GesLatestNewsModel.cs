using System;

namespace GES.Inside.Data.Models
{
    public class GesLatestNewsModel
    {
        public long NewsId { get; set; }
        public string NewsDescription { get; set; }
        public DateTime? NewsLatestNewsModified { get; set; }
        public DateTime? NewsCreated { get; set; }

        public long CaseReportId { get; set; }
        public string CaseReportHeading { get; set; }
        public long EngagementTypeId { get; set; }

        public long GesCompanyId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName{ get; set; }

        public long? EngagementTypeCategoriesId { get; set; }

        public long? ParentForumMessagesId { get; set; }
        public long? ForumMessagesTreeId { get; set; }
    }
}
