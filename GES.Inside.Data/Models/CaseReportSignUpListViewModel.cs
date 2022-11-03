using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class CaseReportSignUpListViewModel
    {
        public long Id { get; set; }

        public long CaseProfileId { get; set; }

        public long CompanyId { get; set; }
        public int SustainalyticsID { get; set; }

        public string CompanyName { get; set; }

        public string CaseName { get; set; }

        public int NumberOfSignUp { get; set; }

        public string OrganizationName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Endorsement { get; set; }

    }
}