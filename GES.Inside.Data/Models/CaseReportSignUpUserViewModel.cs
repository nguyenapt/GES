using System;
using System.Collections.Generic;
using GES.Inside.Data.DataContexts;

namespace GES.Inside.Data.Models
{
    public class CaseReportSignUpUserListViewModel
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string OrganizationName { get; set; }

        public string SignUpValue { get; set; }

    }
}