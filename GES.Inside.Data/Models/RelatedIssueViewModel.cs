using System;
using System.Collections.Generic;

namespace GES.Inside.Data.Models
{
    public class RelatedIssueViewModel
    {
        public long Id { get; set; }
        public long? IssueId { get; set; }
        public long? RelatedId { get; set; }
        public long CompanyId { get; set; }
        public string RelatedCompanyName { get; set; }
        public string RelatedIssueName { get; set; }
    }
}