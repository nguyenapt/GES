using GES.Inside.Data.Models;
using GES.Inside.Data.Models.CaseProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GES.Clients.Web.Models
{
    public class CompanyDialogueViewModel
    {
        public ICaseProfileBaseComponent BaseComponent { get; set; }

        public IList<DialogueModel> Data { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }
    }
}