using System;

namespace Sustainalytics.GSS.Entities
{
    public class IssueIndicatorRemark : Remark
    {
        public Guid IssueIndicatorId { get; set; }

        public IssueIndicator IssueIndicator { get; set; }
    }
}
