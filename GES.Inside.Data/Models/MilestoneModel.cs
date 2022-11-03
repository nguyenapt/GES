using System;

namespace GES.Inside.Data.Models
{
    public class MilestoneModel
    {
        public long MilestoneId { get; set; }
        public string MilestoneDescription { get; set; }
        public DateTime? MilestoneModified { get; set; }
        public DateTime? MileStoneCreated { get; set; }
        public Guid? GesMilestoneTypesId { get; set; }
        public string GesMilestoneTypeName { get; set; }

        public long CaseReportId { get; set; }
        public string CaseReportHeading { get; set; }
        public long EngagementTypeId { get; set; }

        public long GesCompanyId { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }

        public long? EngagementTypeCategoriesId { get; set; }

        public long? ParentForumMessagesId { get; set; }
        public long? ForumMessagesTreeId { get; set; }
    }
}
