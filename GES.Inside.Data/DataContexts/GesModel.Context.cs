//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GES.Inside.Data.DataContexts
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class GesEntities : DbContext
    {
        public GesEntities()
            : base("name=GesEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AlertRatings> AlertRatings { get; set; }
        public virtual DbSet<ControversialRatings> ControversialRatings { get; set; }
        public virtual DbSet<E_MessageContent> E_MessageContent { get; set; }
        public virtual DbSet<E_MessageContentG_Individuals> E_MessageContentG_Individuals { get; set; }
        public virtual DbSet<E_Messages> E_Messages { get; set; }
        public virtual DbSet<E_MessagesR_Lists> E_MessagesR_Lists { get; set; }
        public virtual DbSet<EngagementRatings> EngagementRatings { get; set; }
        public virtual DbSet<F_CreditReportStates> F_CreditReportStates { get; set; }
        public virtual DbSet<G_Bills> G_Bills { get; set; }
        public virtual DbSet<G_ClientDialogue> G_ClientDialogue { get; set; }
        public virtual DbSet<G_Cultures> G_Cultures { get; set; }
        public virtual DbSet<G_Departments> G_Departments { get; set; }
        public virtual DbSet<G_DocumentManagementTaxonomies> G_DocumentManagementTaxonomies { get; set; }
        public virtual DbSet<G_DocumentManagementTaxonomiesG_ManagedDocuments> G_DocumentManagementTaxonomiesG_ManagedDocuments { get; set; }
        public virtual DbSet<G_FileExtensions> G_FileExtensions { get; set; }
        public virtual DbSet<G_Forms> G_Forms { get; set; }
        public virtual DbSet<G_ForumMessages> G_ForumMessages { get; set; }
        public virtual DbSet<G_ForumMessages_Tree> G_ForumMessages_Tree { get; set; }
        public virtual DbSet<G_Genders> G_Genders { get; set; }
        public virtual DbSet<G_Individuals> G_Individuals { get; set; }
        public virtual DbSet<G_IndividualsG_Services> G_IndividualsG_Services { get; set; }
        public virtual DbSet<G_IndividualsGroups> G_IndividualsGroups { get; set; }
        public virtual DbSet<G_Industries> G_Industries { get; set; }
        public virtual DbSet<G_IntNameIds> G_IntNameIds { get; set; }
        public virtual DbSet<G_IntNames> G_IntNames { get; set; }
        public virtual DbSet<G_Languages> G_Languages { get; set; }
        public virtual DbSet<G_LocalizedLongTexts> G_LocalizedLongTexts { get; set; }
        public virtual DbSet<G_LocalizedLongTextTranslations> G_LocalizedLongTextTranslations { get; set; }
        public virtual DbSet<G_LocalizedTexts> G_LocalizedTexts { get; set; }
        public virtual DbSet<G_LocalizedTextTranslations> G_LocalizedTextTranslations { get; set; }
        public virtual DbSet<G_ManagedDocumentApprovalStatuses> G_ManagedDocumentApprovalStatuses { get; set; }
        public virtual DbSet<G_ManagedDocumentItems> G_ManagedDocumentItems { get; set; }
        public virtual DbSet<G_ManagedDocumentRiskLevels> G_ManagedDocumentRiskLevels { get; set; }
        public virtual DbSet<G_ManagedDocuments> G_ManagedDocuments { get; set; }
        public virtual DbSet<G_ManagedDocumentServices> G_ManagedDocumentServices { get; set; }
        public virtual DbSet<G_ManagedDocumentsG_ManagedDocumentTaggs> G_ManagedDocumentsG_ManagedDocumentTaggs { get; set; }
        public virtual DbSet<G_ManagedDocumentTaggs> G_ManagedDocumentTaggs { get; set; }
        public virtual DbSet<G_MarketingMethods> G_MarketingMethods { get; set; }
        public virtual DbSet<G_MimeTypes> G_MimeTypes { get; set; }
        public virtual DbSet<G_MultiLangDescriptions> G_MultiLangDescriptions { get; set; }
        public virtual DbSet<G_MultiLangNames> G_MultiLangNames { get; set; }
        public virtual DbSet<G_News> G_News { get; set; }
        public virtual DbSet<G_NewsCategories> G_NewsCategories { get; set; }
        public virtual DbSet<G_NewsTypes> G_NewsTypes { get; set; }
        public virtual DbSet<G_OrganizationsCalenderEvents> G_OrganizationsCalenderEvents { get; set; }
        public virtual DbSet<G_OrganizationsG_Services> G_OrganizationsG_Services { get; set; }
        public virtual DbSet<G_OrganizationsI_ClientStatuses> G_OrganizationsI_ClientStatuses { get; set; }
        public virtual DbSet<G_Pages> G_Pages { get; set; }
        public virtual DbSet<G_PaymentMethods> G_PaymentMethods { get; set; }
        public virtual DbSet<G_Provinces> G_Provinces { get; set; }
        public virtual DbSet<G_QualityPolicies> G_QualityPolicies { get; set; }
        public virtual DbSet<G_Reminders> G_Reminders { get; set; }
        public virtual DbSet<G_ReportingPageModules> G_ReportingPageModules { get; set; }
        public virtual DbSet<G_ReportingTemplates> G_ReportingTemplates { get; set; }
        public virtual DbSet<G_ReportingTemplatesG_ReportingModules> G_ReportingTemplatesG_ReportingModules { get; set; }
        public virtual DbSet<G_SalesRatings> G_SalesRatings { get; set; }
        public virtual DbSet<G_SaleStates> G_SaleStates { get; set; }
        public virtual DbSet<G_Searches> G_Searches { get; set; }
        public virtual DbSet<G_SecurityGroups> G_SecurityGroups { get; set; }
        public virtual DbSet<G_SecurityGroupsG_Forms> G_SecurityGroupsG_Forms { get; set; }
        public virtual DbSet<G_SecurityGroupsG_Pages> G_SecurityGroupsG_Pages { get; set; }
        public virtual DbSet<G_SecurityGroupsG_SecurityRights> G_SecurityGroupsG_SecurityRights { get; set; }
        public virtual DbSet<G_SecurityRights> G_SecurityRights { get; set; }
        public virtual DbSet<G_Services> G_Services { get; set; }
        public virtual DbSet<G_ServiceStates> G_ServiceStates { get; set; }
        public virtual DbSet<G_StandardTextCategories> G_StandardTextCategories { get; set; }
        public virtual DbSet<G_StandardTexts> G_StandardTexts { get; set; }
        public virtual DbSet<G_Texts> G_Texts { get; set; }
        public virtual DbSet<G_TimeZones> G_TimeZones { get; set; }
        public virtual DbSet<G_Titles> G_Titles { get; set; }
        public virtual DbSet<G_Uploads> G_Uploads { get; set; }
        public virtual DbSet<G_Urls> G_Urls { get; set; }
        public virtual DbSet<G_UserLoginLog> G_UserLoginLog { get; set; }
        public virtual DbSet<G_Users> G_Users { get; set; }
        public virtual DbSet<G_UsersG_SecurityGroups> G_UsersG_SecurityGroups { get; set; }
        public virtual DbSet<G_WebServiceUsers> G_WebServiceUsers { get; set; }
        public virtual DbSet<I_ActivityForms> I_ActivityForms { get; set; }
        public virtual DbSet<I_Answers> I_Answers { get; set; }
        public virtual DbSet<I_BurmaRecommendations> I_BurmaRecommendations { get; set; }
        public virtual DbSet<I_Categories> I_Categories { get; set; }
        public virtual DbSet<I_CharScores> I_CharScores { get; set; }
        public virtual DbSet<I_ClientProgressStatuses> I_ClientProgressStatuses { get; set; }
        public virtual DbSet<I_ClientStatuses> I_ClientStatuses { get; set; }
        public virtual DbSet<I_CompaniesG_Organizations> I_CompaniesG_Organizations { get; set; }
        public virtual DbSet<I_CompaniesG_Services> I_CompaniesG_Services { get; set; }
        public virtual DbSet<I_CompaniesI_CompanyListTypes> I_CompaniesI_CompanyListTypes { get; set; }
        public virtual DbSet<I_CompaniesI_ManagementSystems> I_CompaniesI_ManagementSystems { get; set; }
        public virtual DbSet<I_CompaniesImport> I_CompaniesImport { get; set; }
        public virtual DbSet<I_CompanyListTypes> I_CompanyListTypes { get; set; }
        public virtual DbSet<I_ContactDirections> I_ContactDirections { get; set; }
        public virtual DbSet<I_ContactTypes> I_ContactTypes { get; set; }
        public virtual DbSet<I_ControversialActivites> I_ControversialActivites { get; set; }
        public virtual DbSet<I_ControversialActivitesGroups> I_ControversialActivitesGroups { get; set; }
        public virtual DbSet<I_ControversialComments> I_ControversialComments { get; set; }
        public virtual DbSet<I_ControversialCompanies> I_ControversialCompanies { get; set; }
        public virtual DbSet<I_ControversialCompaniesI_ControversialActivites> I_ControversialCompaniesI_ControversialActivites { get; set; }
        public virtual DbSet<I_ControversialInvolvementTexts> I_ControversialInvolvementTexts { get; set; }
        public virtual DbSet<I_ControversialScreening> I_ControversialScreening { get; set; }
        public virtual DbSet<I_ControversialSources> I_ControversialSources { get; set; }
        public virtual DbSet<I_ControversialSourceTypes> I_ControversialSourceTypes { get; set; }
        public virtual DbSet<I_ConventionCategories> I_ConventionCategories { get; set; }
        public virtual DbSet<I_Conventions> I_Conventions { get; set; }
        public virtual DbSet<I_ConventionSignatories> I_ConventionSignatories { get; set; }
        public virtual DbSet<I_CountriesOfOperation> I_CountriesOfOperation { get; set; }
        public virtual DbSet<I_CountryRisk> I_CountryRisk { get; set; }
        public virtual DbSet<I_CountryRiskScores> I_CountryRiskScores { get; set; }
        public virtual DbSet<I_EngagementActivityOptions> I_EngagementActivityOptions { get; set; }
        public virtual DbSet<I_EngagementDiscussionPoints> I_EngagementDiscussionPoints { get; set; }
        public virtual DbSet<I_EngagementGap> I_EngagementGap { get; set; }
        public virtual DbSet<I_EngagementGoal> I_EngagementGoal { get; set; }
        public virtual DbSet<I_EngagementIssuesInfo> I_EngagementIssuesInfo { get; set; }
        public virtual DbSet<I_EngagementOtherStakeholderViews> I_EngagementOtherStakeholderViews { get; set; }
        public virtual DbSet<I_EngagementProfiles> I_EngagementProfiles { get; set; }
        public virtual DbSet<I_EngagementStandards> I_EngagementStandards { get; set; }
        public virtual DbSet<I_EngagementSwots> I_EngagementSwots { get; set; }
        public virtual DbSet<I_EngagementTypeCategories> I_EngagementTypeCategories { get; set; }
        public virtual DbSet<I_EngagementTypeNews> I_EngagementTypeNews { get; set; }
        public virtual DbSet<I_Ftse_Tree> I_Ftse_Tree { get; set; }
        public virtual DbSet<I_FtseGroups> I_FtseGroups { get; set; }
        public virtual DbSet<I_FtseSectors> I_FtseSectors { get; set; }
        public virtual DbSet<I_FtseSubSectors> I_FtseSubSectors { get; set; }
        public virtual DbSet<I_GesAssociatedCorporations> I_GesAssociatedCorporations { get; set; }
        public virtual DbSet<I_GesCaseReportAvailabilityStatuses> I_GesCaseReportAvailabilityStatuses { get; set; }
        public virtual DbSet<I_GesCaseReportHistory> I_GesCaseReportHistory { get; set; }
        public virtual DbSet<I_GesCaseReportReferences> I_GesCaseReportReferences { get; set; }
        public virtual DbSet<I_GesCaseReportsG_Individuals> I_GesCaseReportsG_Individuals { get; set; }
        public virtual DbSet<I_GesCaseReportsI_Conventions> I_GesCaseReportsI_Conventions { get; set; }
        public virtual DbSet<I_GesCaseReportsI_EngagementTypes> I_GesCaseReportsI_EngagementTypes { get; set; }
        public virtual DbSet<I_GesCaseReportsI_GesAssociatedCorporations> I_GesCaseReportsI_GesAssociatedCorporations { get; set; }
        public virtual DbSet<I_GesCaseReportsI_GesRevisionTexts> I_GesCaseReportsI_GesRevisionTexts { get; set; }
        public virtual DbSet<I_GesCaseReportsI_Kpis> I_GesCaseReportsI_Kpis { get; set; }
        public virtual DbSet<I_GesCaseReportsI_Norms> I_GesCaseReportsI_Norms { get; set; }
        public virtual DbSet<I_GesCaseReportsI_ProcessStatuses> I_GesCaseReportsI_ProcessStatuses { get; set; }
        public virtual DbSet<I_GesCaseReportSources> I_GesCaseReportSources { get; set; }
        public virtual DbSet<I_GesCaseReportSourceTypes> I_GesCaseReportSourceTypes { get; set; }
        public virtual DbSet<I_GesCaseReportStatuses> I_GesCaseReportStatuses { get; set; }
        public virtual DbSet<I_GesCaseReportSupplementaryReading> I_GesCaseReportSupplementaryReading { get; set; }
        public virtual DbSet<I_GesCaseReportUpdates> I_GesCaseReportUpdates { get; set; }
        public virtual DbSet<I_GesCommentary> I_GesCommentary { get; set; }
        public virtual DbSet<I_GesCompanies> I_GesCompanies { get; set; }
        public virtual DbSet<I_GesCompanyDialogues> I_GesCompanyDialogues { get; set; }
        public virtual DbSet<I_GesCompanyDialoguesG_Individuals> I_GesCompanyDialoguesG_Individuals { get; set; }
        public virtual DbSet<I_GesCompanyWatcher> I_GesCompanyWatcher { get; set; }
        public virtual DbSet<I_GesLatestNews> I_GesLatestNews { get; set; }
        public virtual DbSet<I_GesRevisionTexts> I_GesRevisionTexts { get; set; }
        public virtual DbSet<I_GesSourceDialogues> I_GesSourceDialogues { get; set; }
        public virtual DbSet<I_GesSourceDialoguesG_Individuals> I_GesSourceDialoguesG_Individuals { get; set; }
        public virtual DbSet<I_HighRiskZones> I_HighRiskZones { get; set; }
        public virtual DbSet<I_HrCommunityRatingsConversionTable> I_HrCommunityRatingsConversionTable { get; set; }
        public virtual DbSet<I_HrEmployeesRatingsConversionTable> I_HrEmployeesRatingsConversionTable { get; set; }
        public virtual DbSet<I_HrRatingsConversionTable> I_HrRatingsConversionTable { get; set; }
        public virtual DbSet<I_HrSuppliersRatingsConversionTable> I_HrSuppliersRatingsConversionTable { get; set; }
        public virtual DbSet<I_HrTotalRatingsConversionTable> I_HrTotalRatingsConversionTable { get; set; }
        public virtual DbSet<I_IndividualLists> I_IndividualLists { get; set; }
        public virtual DbSet<I_IndividualListsG_Individuals> I_IndividualListsG_Individuals { get; set; }
        public virtual DbSet<I_InformationQuality> I_InformationQuality { get; set; }
        public virtual DbSet<I_KpiPerformance> I_KpiPerformance { get; set; }
        public virtual DbSet<I_Kpis> I_Kpis { get; set; }
        public virtual DbSet<I_ManagementSystems> I_ManagementSystems { get; set; }
        public virtual DbSet<I_ManagementSystemTypes> I_ManagementSystemTypes { get; set; }
        public virtual DbSet<I_Milestones> I_Milestones { get; set; }
        public virtual DbSet<I_Msci_Tree> I_Msci_Tree { get; set; }
        public virtual DbSet<I_MsciI_Questions> I_MsciI_Questions { get; set; }
        public virtual DbSet<I_MsciIndustries> I_MsciIndustries { get; set; }
        public virtual DbSet<I_MsciIndustryGroups> I_MsciIndustryGroups { get; set; }
        public virtual DbSet<I_MsciSectors> I_MsciSectors { get; set; }
        public virtual DbSet<I_MsciSubIndustries> I_MsciSubIndustries { get; set; }
        public virtual DbSet<I_Nas> I_Nas { get; set; }
        public virtual DbSet<I_NasI_IndividualLists> I_NasI_IndividualLists { get; set; }
        public virtual DbSet<I_NasI_NaArticles> I_NasI_NaArticles { get; set; }
        public virtual DbSet<I_NaTypes> I_NaTypes { get; set; }
        public virtual DbSet<I_NormAreas> I_NormAreas { get; set; }
        public virtual DbSet<I_NormCategories> I_NormCategories { get; set; }
        public virtual DbSet<I_Norms> I_Norms { get; set; }
        public virtual DbSet<I_Portfolios> I_Portfolios { get; set; }
        public virtual DbSet<I_PortfoliosG_Organizations> I_PortfoliosG_Organizations { get; set; }
        public virtual DbSet<I_PortfoliosG_OrganizationsG_Services> I_PortfoliosG_OrganizationsG_Services { get; set; }
        public virtual DbSet<I_PortfoliosG_OrganizationsI_ControversialActivites> I_PortfoliosG_OrganizationsI_ControversialActivites { get; set; }
        public virtual DbSet<I_PortfolioTypes> I_PortfolioTypes { get; set; }
        public virtual DbSet<I_ProcessMembers> I_ProcessMembers { get; set; }
        public virtual DbSet<I_ProcessStatuses> I_ProcessStatuses { get; set; }
        public virtual DbSet<I_ProgressStatuses> I_ProgressStatuses { get; set; }
        public virtual DbSet<I_ProxyResolutions> I_ProxyResolutions { get; set; }
        public virtual DbSet<I_Questions> I_Questions { get; set; }
        public virtual DbSet<I_QuestionsByRisk> I_QuestionsByRisk { get; set; }
        public virtual DbSet<I_RatingsConversionTable> I_RatingsConversionTable { get; set; }
        public virtual DbSet<I_RelatedCases> I_RelatedCases { get; set; }
        public virtual DbSet<I_ReportFiles> I_ReportFiles { get; set; }
        public virtual DbSet<I_ResponseStatuses> I_ResponseStatuses { get; set; }
        public virtual DbSet<I_RiskCompanies> I_RiskCompanies { get; set; }
        public virtual DbSet<I_RiskCompaniesStatistics> I_RiskCompaniesStatistics { get; set; }
        public virtual DbSet<I_RiskIndustryAverageScores> I_RiskIndustryAverageScores { get; set; }
        public virtual DbSet<I_RiskMatrixTexts> I_RiskMatrixTexts { get; set; }
        public virtual DbSet<I_RiskStatistics> I_RiskStatistics { get; set; }
        public virtual DbSet<I_Scores> I_Scores { get; set; }
        public virtual DbSet<I_Sections> I_Sections { get; set; }
        public virtual DbSet<I_SecurityEvents> I_SecurityEvents { get; set; }
        public virtual DbSet<I_SecurityStatuses> I_SecurityStatuses { get; set; }
        public virtual DbSet<I_SecurityTypes> I_SecurityTypes { get; set; }
        public virtual DbSet<I_Sources> I_Sources { get; set; }
        public virtual DbSet<I_Swots> I_Swots { get; set; }
        public virtual DbSet<I_SwotScores> I_SwotScores { get; set; }
        public virtual DbSet<I_SwotTypes> I_SwotTypes { get; set; }
        public virtual DbSet<I_Themes> I_Themes { get; set; }
        public virtual DbSet<I_TimelineItems> I_TimelineItems { get; set; }
        public virtual DbSet<L_UsersUsers> L_UsersUsers { get; set; }
        public virtual DbSet<M_Messages> M_Messages { get; set; }
        public virtual DbSet<M_MessagesI_IndividualLists> M_MessagesI_IndividualLists { get; set; }
        public virtual DbSet<M_MessageTypes> M_MessageTypes { get; set; }
        public virtual DbSet<O_AlertStatuses> O_AlertStatuses { get; set; }
        public virtual DbSet<O_AnalysisLevels> O_AnalysisLevels { get; set; }
        public virtual DbSet<O_Comments> O_Comments { get; set; }
        public virtual DbSet<O_Groups> O_Groups { get; set; }
        public virtual DbSet<O_Inboxes> O_Inboxes { get; set; }
        public virtual DbSet<O_Links> O_Links { get; set; }
        public virtual DbSet<O_NewsItemFiles> O_NewsItemFiles { get; set; }
        public virtual DbSet<O_NewsItemLinks> O_NewsItemLinks { get; set; }
        public virtual DbSet<O_NewsItems> O_NewsItems { get; set; }
        public virtual DbSet<O_NewsItemsG_Services> O_NewsItemsG_Services { get; set; }
        public virtual DbSet<O_NewsItemsI_Companies> O_NewsItemsI_Companies { get; set; }
        public virtual DbSet<O_NewsItemsO_Taxonomy> O_NewsItemsO_Taxonomy { get; set; }
        public virtual DbSet<O_ResultTypes> O_ResultTypes { get; set; }
        public virtual DbSet<O_SearchTerms> O_SearchTerms { get; set; }
        public virtual DbSet<O_Sources> O_Sources { get; set; }
        public virtual DbSet<O_Taxonomy> O_Taxonomy { get; set; }
        public virtual DbSet<O_TaxonomyTree> O_TaxonomyTree { get; set; }
        public virtual DbSet<R_EmailSource> R_EmailSource { get; set; }
        public virtual DbSet<R_Lists> R_Lists { get; set; }
        public virtual DbSet<R_ListsG_Individuals> R_ListsG_Individuals { get; set; }
        public virtual DbSet<R_ListsG_Organizations> R_ListsG_Organizations { get; set; }
        public virtual DbSet<RiskRatings> RiskRatings { get; set; }
        public virtual DbSet<StandardRatings> StandardRatings { get; set; }
        public virtual DbSet<T_Activites> T_Activites { get; set; }
        public virtual DbSet<T_ActivitesI_GesCaseReports> T_ActivitesI_GesCaseReports { get; set; }
        public virtual DbSet<T_ActivityTypes> T_ActivityTypes { get; set; }
        public virtual DbSet<T_ActMan_Start> T_ActMan_Start { get; set; }
        public virtual DbSet<T_ApprovalStatuses> T_ApprovalStatuses { get; set; }
        public virtual DbSet<T_Projects> T_Projects { get; set; }
        public virtual DbSet<T_Translate> T_Translate { get; set; }
        public virtual DbSet<T_Translated> T_Translated { get; set; }
        public virtual DbSet<G_OrganizationsG_ManagedDocuments> G_OrganizationsG_ManagedDocuments { get; set; }
        public virtual DbSet<I_CalenderEvents_Audit> I_CalenderEvents_Audit { get; set; }
        public virtual DbSet<I_GesCaseReportsI_GesRevisionTexts_Audit> I_GesCaseReportsI_GesRevisionTexts_Audit { get; set; }
        public virtual DbSet<I_GesCaseReportsI_Kpis_Audit> I_GesCaseReportsI_Kpis_Audit { get; set; }
        public virtual DbSet<I_GesCaseReportsI_ProcessStatuses_Audit> I_GesCaseReportsI_ProcessStatuses_Audit { get; set; }
        public virtual DbSet<I_PdfExportSettings> I_PdfExportSettings { get; set; }
        public virtual DbSet<I_PortfoliosI_ControversialActivites> I_PortfoliosI_ControversialActivites { get; set; }
        public virtual DbSet<I_RiskRankTexts> I_RiskRankTexts { get; set; }
        public virtual DbSet<O_NewsItemsArchive> O_NewsItemsArchive { get; set; }
        public virtual DbSet<O_Texts> O_Texts { get; set; }
        public virtual DbSet<I_ControversialActivitesPresets> I_ControversialActivitesPresets { get; set; }
        public virtual DbSet<I_ControversialActivitesPresetsItems> I_ControversialActivitesPresetsItems { get; set; }
        public virtual DbSet<I_PortfoliosI_Companies> I_PortfoliosI_Companies { get; set; }
        public virtual DbSet<I_GesCaseReportsExtra> I_GesCaseReportsExtra { get; set; }
        public virtual DbSet<GesCaseReportSignUp> GesCaseReportSignUp { get; set; }
        public virtual DbSet<G_Organizations_GesDocument> G_Organizations_GesDocument { get; set; }
        public virtual DbSet<GesDocument> GesDocument { get; set; }
        public virtual DbSet<GesDocumentService> GesDocumentService { get; set; }
        public virtual DbSet<GesCompanyProfiles> GesCompanyProfiles { get; set; }
        public virtual DbSet<I_CompaniesG_ManagedDocuments> I_CompaniesG_ManagedDocuments { get; set; }
        public virtual DbSet<GesCaseReportSdg> GesCaseReportSdg { get; set; }
        public virtual DbSet<Sdg> Sdg { get; set; }
        public virtual DbSet<I_EngagementTypes_GesDocument> I_EngagementTypes_GesDocument { get; set; }
        public virtual DbSet<I_GesCaseReports_Audit> I_GesCaseReports_Audit { get; set; }
        public virtual DbSet<GesMilestoneTypes> GesMilestoneTypes { get; set; }
        public virtual DbSet<GesUNGPAssessmentScores> GesUNGPAssessmentScores { get; set; }
        public virtual DbSet<GesUNGPAssessmentForm> GesUNGPAssessmentForm { get; set; }
        public virtual DbSet<GesUNGPAssessmentFormResources> GesUNGPAssessmentFormResources { get; set; }
        public virtual DbSet<GesDocTypes> GesDocTypes { get; set; }
        public virtual DbSet<GesAnnouncement> GesAnnouncement { get; set; }
        public virtual DbSet<GesUNGPAssessmentForm_Audit> GesUNGPAssessmentForm_Audit { get; set; }
        public virtual DbSet<GesUNGPAssessmentForm_Audit_Details> GesUNGPAssessmentForm_Audit_Details { get; set; }
        public virtual DbSet<GesUNGPAssessmentFormResource_Audit> GesUNGPAssessmentFormResource_Audit { get; set; }
        public virtual DbSet<GesUNGPAssessmentFormResource_Audit_Details> GesUNGPAssessmentFormResource_Audit_Details { get; set; }
        public virtual DbSet<GesEventCalendarUserAccept> GesEventCalendarUserAccept { get; set; }
        public virtual DbSet<I_CalenderEvents> I_CalenderEvents { get; set; }
        public virtual DbSet<GesCaseReports_Audit> GesCaseReports_Audit { get; set; }
        public virtual DbSet<GesCaseReports_Audit_Details> GesCaseReports_Audit_Details { get; set; }
        public virtual DbSet<GesCaseProfileInvisibleEntities> GesCaseProfileInvisibleEntities { get; set; }
        public virtual DbSet<I_GesCaseProfileEntities> I_GesCaseProfileEntities { get; set; }
        public virtual DbSet<I_GesCaseProfileEntitiesGroup> I_GesCaseProfileEntitiesGroup { get; set; }
        public virtual DbSet<GesCaseProfileTemplates> GesCaseProfileTemplates { get; set; }
        public virtual DbSet<I_GesCaseReportsG_ManagedDocuments> I_GesCaseReportsG_ManagedDocuments { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<I_Companies> I_Companies { get; set; }
        public virtual DbSet<PeerGroups> PeerGroups { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<SubPeerGroups> SubPeerGroups { get; set; }
        public virtual DbSet<G_Organizations> G_Organizations { get; set; }
        public virtual DbSet<I_NaArticles> I_NaArticles { get; set; }
        public virtual DbSet<I_GSSLink> I_GSSLink { get; set; }
        public virtual DbSet<I_GesCaseReports> I_GesCaseReports { get; set; }
        public virtual DbSet<IssueHeading> IssueHeading { get; set; }
        public virtual DbSet<IssueHeadingGroup> IssueHeadingGroup { get; set; }
        public virtual DbSet<I_PortfolioCompaniesImport> I_PortfolioCompaniesImport { get; set; }
        public virtual DbSet<I_EngagementTypes> I_EngagementTypes { get; set; }
    }
}
