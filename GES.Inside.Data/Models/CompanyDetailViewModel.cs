using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GES.Inside.Data.Models
{
    public class CompanyDetailViewModel
    {
        //Support info
        public long? GesCompanyId { get; set; }
        public long OrganizationId { get; set; }
        public long IndividualId { get; set; }
        
        //Company info
        public long? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Isin { get; set; }
        public string Sedol { get; set; }
        public string BloombergTicker { get; set; }
        public Guid? CountryId { get; set; }
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string OwnershipInfo { get; set; }
        public string MsciIndustry { get; set; }
        public string GicsSector { get; set; }
        public string Website { get; set; }
        
        public string RelevantInitiative { get; set; }
        public string Overview { get; set; }
        public string Industry { get; set; }
        //Key ESG Issues in the industry
        public string KeyEsgIssues { get; set; }
        
        //Company Assessment
        public string CompanyAssessment { get; set; }

        //Engagement Statistics
        public int Correspondence { get; set; }
        public int Contacts { get; set; }
        public int Meetings { get; set; }
        public int Dialogues { get; set; }
        public int ConferenceCalls { get; set; }
        
        public long? Msci_Id { get; set; }
        public long? Ftse_Id { get; set; }

        public int? SubPeerGroupId { get; set; }
        public string SubPeerGroup { get; set; }

        public string UnGlobalCompact { get; set; }
        public string GriAlignedDisclosure { get; set; }

        //Calendar events
        public IList<EventListViewModel> Events { get; set; }

        //Documents
        public IList<DocumentViewModel> Documents { get; set; }

        //Company Information
        public string OtherName1 { get; set; }
        public string OtherName2 { get; set; }
        public string OtherName3 { get; set; }
        public string OldName { get; set; }
        public string MediaName { get; set; }
        public long? MasterCompanyId{ get; set; }
        public string MasterCompanyName { get; set; }
        //public string MostMaterialRisk { get; set; }
        public string TransparencyDisclosure { get; set; }
        //public string SEDOL { get; set; }
        public MsciFtseViewModel FTSE { get; set; }
        //public long? MSCI { get; set; }
        public string InformationSource { get; set; }
        public string ListSource { get; set; }
        public string SecurityDescription { get; set; }
        //public long? CountryRegId { get; set; }
        public MsciFtseViewModel MSCI { get; set; }


        //Personal settings
        public bool IsInFocusList { get; set; }

        [Display(Name = "Distribution of Ratings within the Industry")]
        public string Distribution { get; set; }
        [Display(Name = "Key Issue Performance")]
        public string KeyIssues { get; set; }
        [Display(Name = "Grade")]
        public string Grade { get; set; }
        [Display(Name = "Prime threshold")]
        public string PrimeThreshold { get; set; }
        [Display(Name = "Is prime")]
        public bool? IsPrime { get; set; }
        [Display(Name = "Bloomberg Ticker ID")]
        public string BbgID { get; set; }

        public IList<CompanyManagementSystemModel> CompanyManagementSystems { get; set; }

        public IList<CompanyPortfolioModel> CompanyPortfolios { get; set; }
        public IList<CompanyPortfolioModel> CompanyFollowingStandardPortfolios { get; set; }
        public IList<CompanyPortfolioModel> CompanyFollowingOtherPortfolios { get; set; }
        public IList<CompanyPortfolioModel> CompanyFollowingCustomerPortfolios { get; set; }

        public DateTime? IsParkedForGssResearchSince { get; set; }

    }

    public class ExportCompanyDetailViewModel
    {
        public CompanyDetailViewModel CompanyDetailViewModel { get; set; }
        public IList<CaseReportListViewModel> CaseReportListViewModels { get; set; }
        public IList<AlertListViewModel> AlertListViewModels { get; set; }
        public bool ShowCompanyInfo { get; set; }
        public bool ShowDialogue { get; set; }
        public bool ShowCompanyOverview { get; set; }
        public bool ShowCaseProfiles { get; set; }
        public bool ShowAlerts { get; set; }
        public bool ShowCompanyEvents { get; set; }
        public bool ShowCorporateRatingInformation { get; set; }
        public bool ShowCoverPage { get; set; }
        public bool ShowDocuments { get; set; }
    }
}