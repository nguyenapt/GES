using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using com.sun.org.apache.bcel.@internal.generic;

namespace GES.Inside.Data.Models
{
    public class EngagementTypeViewModel
    {
        public long I_EngagementTypes_Id { get; set; }
        public long I_EngagementTypeCategories_Id { get; set; }
        public string CatalogName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Display(Name = "Engagement goal")]
        public string Goal { get; set; }
        [Display(Name = "Next step")]
        public string NextStep { get; set; }

        [Display(Name = "Latest news")]
        public string LatestNews { get; set; }
        [Display(Name = "Other initiatives")]
        public string OtherInitiatives { get; set; }
        public string Sources { get; set; }
        [Display(Name = "Sustainalytics reports")]
        public string GesReports { get; set; }
        public long? ContactG_Users_Id { get; set; }
        public string Participants { get; set; }
        public string NonSubscriberInformation { get; set; }
        public int? SortOrder { get; set; }
        public DateTime? Created { get; set; }

        public long? ServicesId { get; set; }
        public IList<EventListViewModel> TimeLine { get; set; }

        public bool IsSubscribe { get; set; }

        public string Contact { get; set; }
        public string ContactFullName { get; set; }
        public string ContactMobile { get; set; }
        public string ContactEmail { get; set; }

        public string ThemeImagePath { get; set; }
        public string ThemeImage { get; set; }
        public bool? Deactive { get; set; }
        public bool? IsShowInClientMenu { get; set; }
        public bool? IsShowInCaseProfileTemplate { get; set; }
        

        public IList<KpiViewModel> KPIs { get; set; }
        public IList<EngagementTypeNewsViewModel> EngagementTypeNews { get; set; }
        public IList<EngagementTypeGesDocumentViewModel> EngagementTypeDocuments { get; set; }
        public IList<HttpPostedFileBase> UploadingDocsBases { get; set; }

        public string GovernanceServicesIds { get; set; }

    }
}
