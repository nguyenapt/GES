using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Models
{
    public class CaseReportViewModel
    {
        
        public long Id { get; set; }

        //Company info
        public long CompanyId { get; set; }
        [Display(Name = "Company")]
        public string CompanyName { get; set; }
        [Display(Name = "SEDOL")]
        public string Sedol { get; set; }
        [Display(Name = "ISIN")]
        public string Isin { get; set; }
        [Display(Name = "Industry")]
        public string Industry { get; set; }
        [Display(Name = "Material ESG Risk")]
        public string MaterialEsgRisk { get; set; }
        [Display(Name = "Domicile")]
        public string HomeCountry { get; set; }
        [Display(Name = "UN Global Compact")]
        public bool? UnGlobalCompact { get; set; }

        //Engagement Background
                                    
        [Display(Name = "Issue")]
        public string IssueName { get; set; }
        [Display(Name = "Location")]
        public string Location  { get; set; }
        [Display(Name = "Norm Area")]
        public string NormArea  { get; set; }

        [Display(Name = "Confirmed Violation")]
        public bool? ConfirmedViolation  { get; set; }
        [Display(Name = "Engagement status")]
        public string Recommendation  { get; set; }
        [Display(Name = "Summary")]
        public string Summary  { get; set; }

        //Ges' Commentary
        public string GesComment  { get; set; }

        //Change Objective and Perfomance
        public string ChangeObjective  { get; set; }
        public string Milestone  { get; set; }
        public string NextStep  { get; set; }

        //News and Dialogue
        [Display(Name = "Latest news")]
        public string LatestNews  { get; set; }
        [Display(Name = "Company dialogue")]
        public string CompanyDialogue  { get; set; }

        //Related Issues (other companies)
        public List<RelatedIssueViewModel> RelatedIssues { get; set; } 
        //Status

        //Engagement Statistics
        public int ConferenceCalls { get; set; }
        public int MeetingsInPerson { get; set; }
        public DateTime? LatestMeeting { get; set; }

        //Paticipants

        //Calendar

        //Reports
        public List<RelatedReportViewModel> Reports { get; set;}
        //Portfolios
        public List<IdNameModel> Portfolios { get; set; } 

        //Additional Documents

        //Ges contact information
        
        //Additional issues

    }
}