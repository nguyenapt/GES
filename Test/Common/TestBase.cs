using System;
using System.Collections.Generic;
using GES.Common.Logging;
using GES.Inside.Data.Configs;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Repository;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services;
using Moq;
using NUnit.Framework;
using Z.EntityFramework.Plus;

namespace Test.Common
{
    [TestFixture]
    public abstract class TestBase
    {
        protected Mock<GesEntities> GesEntities;
        protected Mock<IStoredProcedureRunner> StoredProcedureRunner;
        public Mock<IGesLogger> Logger => new Mock<IGesLogger>();
        public I_GesCaseReportsRepository GesCaseReportsRepository => new I_GesCaseReportsRepository(GesEntities.Object, Logger.Object, ConventionsRepository, CompaniesRepository, null, null);
        public G_ManagedDocumentsRepository ManagedDocumentsRepository => new G_ManagedDocumentsRepository(GesEntities.Object, Logger.Object);
        public GesCaseReportSignUpRepository CaseReportSignUpRepository => new GesCaseReportSignUpRepository(GesEntities.Object, Logger.Object);
        public I_ConventionsRepository ConventionsRepository => new I_ConventionsRepository(GesEntities.Object, Logger.Object);
        public I_CompaniesRepository CompaniesRepository => new I_CompaniesRepository(GesEntities.Object, Logger.Object, StoredProcedureRunner.Object);
        public UnitOfWork<GesEntities> DbContextGesEntities => new UnitOfWork<GesEntities>(GesEntities.Object, Logger.Object);
        public I_CalendarEventsRepository CalendarReporsitory => new I_CalendarEventsRepository(GesEntities.Object, Logger.Object);
        public CalendarService CalendarService => new CalendarService(DbContextGesEntities, CalendarReporsitory, Logger.Object);
        public I_GesCaseReportsExtraRepository GesCaseReportsExtraRepository => new I_GesCaseReportsExtraRepository(GesEntities.Object, Logger.Object);

        [SetUp]
        public virtual void SetUp()
        {
            AutoMapperDataConfiguration.Configure();
            GesEntities = new Mock<GesEntities>();
            StoredProcedureRunner = new Mock<IStoredProcedureRunner>();
            QueryCacheManager.CacheKeyFactory = (query, tags) => query.ToString() + string.Join(";", tags);
            PrepareDefaultData();
        }
        
        private void PrepareDefaultData()
        {
            PrepareDatabaseData();
            PrepareStoredProcedureData();
        }
        
        private void PrepareDatabaseData()
        {
            var companyData = new List<I_Companies>
            {
                new I_Companies { I_Companies_Id = 1, I_Msci_Id = 1, Isin = "Isin1", Name = "Company 1", I_Companies2 = new I_Companies {I_Companies_Id = 1, Name = "Sub Company 1"}, Countries = new Countries {Name = "Sweden"} },
                new I_Companies { I_Companies_Id = 2, I_Msci_Id = 1, Isin = "Isin2", MasterI_Companies_Id = 1, Name = "Company 2", I_Companies2 = new I_Companies {I_Companies_Id = 1, Name = "Sub Company 1"}, Countries = new Countries {Name = "Sweden"} },
                new I_Companies { I_Companies_Id = 3, I_Msci_Id = 1, Isin = "Isin3", MasterI_Companies_Id = 1, Name = "Company 3", I_Companies2 = new I_Companies {I_Companies_Id = 1, Name = "Sub Company 1"}, Countries = new Countries {Name = "Sweden"} }
            };
            var gesCompanyData = new List<I_GesCompanies>
            {
                new I_GesCompanies {I_GesCompanies_Id = 1, I_Companies_Id = 1}
            };
            var caseReportData = new List<I_GesCaseReports>
            {
                new I_GesCaseReports { I_GesCaseReports_Id = 1, ReportIncident = "Incident 1", I_GesCompanies_Id = 1, LocationG_Countries_Id = 1, I_NormAreas_Id = 1, NewI_GesCaseReportStatuses_Id = 1, I_GesCaseReportStatuses_Id = 1, I_ResponseStatuses_Id = 1, I_ProgressStatuses_Id = 1, AnalystG_Users_Id = 1, ShowInClient = true, Confirmed = true, Summary = "" },
                new I_GesCaseReports { I_GesCaseReports_Id = 2, ReportIncident = "Incident 2", I_GesCompanies_Id = 1, LocationG_Countries_Id = 1, I_NormAreas_Id = 1, NewI_GesCaseReportStatuses_Id = 2, I_GesCaseReportStatuses_Id = 1, I_ResponseStatuses_Id = 1, I_ProgressStatuses_Id = 1, AnalystG_Users_Id = 1, ShowInClient = true, Confirmed = true, Summary = "" },
                new I_GesCaseReports { I_GesCaseReports_Id = 3, ReportIncident = "Incident 2", I_GesCompanies_Id = 1, LocationG_Countries_Id = 1, I_NormAreas_Id = 1, NewI_GesCaseReportStatuses_Id = 1, I_GesCaseReportStatuses_Id = 1, I_ResponseStatuses_Id = 1, I_ProgressStatuses_Id = 1, AnalystG_Users_Id = 1, ShowInClient = true, Confirmed = true, Summary = "" }
            };
            var countryData = new List<Countries>
            {
                new Countries {Id = Guid.Empty, Name = "Sweden"}
            };
            var msciData = new List<SubPeerGroups>
            {
                new SubPeerGroups() {Id = 1}
            };
            var normAreadData = new List<I_NormAreas>
            {
                new I_NormAreas {I_NormAreas_Id = 1, Name = "Environment"}
            };
            var commentaryData = new List<I_GesCommentary>
            {
                new I_GesCommentary {I_GesCaseReports_Id = 1}
            };
            var recommendationData = new List<I_GesCaseReportStatuses>
            {
                new I_GesCaseReportStatuses {I_GesCaseReportStatuses_Id = 1, Name = "Engagement", SortOrder = 1},
                new I_GesCaseReportStatuses {I_GesCaseReportStatuses_Id = 2, Name = "Engagement", SortOrder = 2}
            };
            var dialogueData = new List<I_GesCompanyDialogues>
            {
                new I_GesCompanyDialogues {I_GesCaseReports_Id = 1, I_GesCompanyDialogues_Id = 1, G_Individuals_Id = 1, I_ContactTypes_Id = 1, G_ManagedDocuments_Id = 1},
                new I_GesCompanyDialogues {I_GesCaseReports_Id = 2, I_GesCompanyDialogues_Id = 1, G_Individuals_Id = 1, I_ContactTypes_Id = 1, G_ManagedDocuments_Id = 1}
            };
            var engagementTypeData = new List<I_GesCaseReportsI_EngagementTypes>
            {
                new I_GesCaseReportsI_EngagementTypes {I_GesCaseReports_Id = 1, I_EngagementTypes_Id = 1},
                new I_GesCaseReportsI_EngagementTypes {I_GesCaseReports_Id = 2, I_EngagementTypes_Id = 1},
                new I_GesCaseReportsI_EngagementTypes {I_GesCaseReports_Id = 3, I_EngagementTypes_Id = 1}
            };
            var serviceData = new List<G_Services>
            {
                new G_Services {I_EngagementTypes_Id = 1, G_Services_Id = 1}
            };
            var organizationsService = new List<G_OrganizationsG_Services>
            {
                new G_OrganizationsG_Services {G_Services_Id = 1, G_OrganizationsG_Services_Id = 1, G_Organizations_Id = 1}
            };
            var latestNewsData = new List<I_GesLatestNews>
            {
                new I_GesLatestNews {I_GesCaseReports_Id = 1, Description = "Latest new content"}
            };
            var usersData = new List<G_Users>
            {
                new G_Users {G_Users_Id = 1, G_Individuals_Id = 1}
            };
            var individualData = new List<G_Individuals>
            {
                new G_Individuals { G_Individuals_Id = 1, FirstName = "Hanna", LastName = "Robert", Email = "hanna.robert@ges.com", JobTitle = "Product Owner", G_Organizations_Id = 1}
            };
            var manageDocumentData = new List<I_CompaniesG_ManagedDocuments>
            {
                new I_CompaniesG_ManagedDocuments {G_ManagedDocuments_Id = 1, I_Companies_Id = 1}
            };
            var uploadData = new List<G_Uploads>
            {
                new G_Uploads {G_Uploads_Id = 1, G_ManagedDocuments_Id = 1}
            };
            var gesCaseReportSignUpData = new List<GesCaseReportSignUp>
            {
                new GesCaseReportSignUp {G_Individuals_Id = 1}
            };
            var engagementDiscussionPoints = new List<I_EngagementDiscussionPoints>
            {
                new I_EngagementDiscussionPoints {I_EngagementDiscussionPoints_Id = 1, I_Companies_Id = 1}
            };
            var engagementOtherStakeholderViews = new List<I_EngagementOtherStakeholderViews>
            {
                new I_EngagementOtherStakeholderViews {I_EngagementOtherStakeholderViews_Id = 1, I_Companies_Id = 1}
            };
            var engagementProfiles = new List<I_EngagementProfiles>
            {
                new I_EngagementProfiles {I_EngagementProfiles_Id = 1, I_Companies_Id = 1}
            };
            var calendarEvents = new List<I_CalenderEvents>
            {
                new I_CalenderEvents {I_CalenderEvents_Id = 1, I_Companies_Id = 1}
            };
            var responseStatuses = new List<I_ResponseStatuses>
            {
                new I_ResponseStatuses {I_ResponseStatuses_Id = 1}
            };
            var progressStatuses = new List<I_ProgressStatuses>
            {
                new I_ProgressStatuses {I_ProgressStatuses_Id = 1}
            };

            var engagementTypes = new List<I_EngagementTypes>
            {
                new I_EngagementTypes {I_EngagementTypes_Id = 1, I_EngagementTypeCategories_Id = 1, Name = "Environment Engagement"}
            };

            var engagementTypeCategories = new List<I_EngagementTypeCategories>
            {
                new I_EngagementTypeCategories {I_EngagementTypeCategories_Id = 1, Name = "Global Standards"}
            };

            var portfoliosOrganizations = new List<I_PortfoliosG_Organizations>
            {
                new I_PortfoliosG_Organizations { I_PortfoliosG_Organizations_Id = 1, I_Portfolios_Id = 1, G_Organizations_Id = 1 }
            };

            var portfoliosOrganizationsServices = new List<I_PortfoliosG_OrganizationsG_Services>
            {
                new I_PortfoliosG_OrganizationsG_Services {I_PortfoliosG_Organizations_Id = 1, G_Services_Id = 1}
            };

            var portfoliosCompanies = new List<I_PortfoliosI_Companies>
            {
                new I_PortfoliosI_Companies {I_Portfolios_Id = 1, I_Companies_Id = 1}
            };

            var organizationsServices = new List<G_OrganizationsG_Services>
            {
                new G_OrganizationsG_Services {G_Organizations_Id = 1, G_Services_Id = 1}
            };

            var gesCaseReportsProcessStatuses = new List<I_GesCaseReportsI_ProcessStatuses>
            {
                new I_GesCaseReportsI_ProcessStatuses {I_GesCaseReports_Id = 1, I_ProcessStatuses_Id = 13},
                new I_GesCaseReportsI_ProcessStatuses {I_GesCaseReports_Id = 2, I_ProcessStatuses_Id = 13},
                new I_GesCaseReportsI_ProcessStatuses {I_GesCaseReports_Id = 3, I_ProcessStatuses_Id = 13}
            };
            var milestones = new List<I_Milestones>
            {
                new I_Milestones {I_Milestones_Id = 1, I_GesCaseReports_Id = 1}
            };
            var gesCaseReportsExtra = new List<I_GesCaseReportsExtra>
            {
                new I_GesCaseReportsExtra {I_GesCaseReports_Id = 1, I_GesCaseReportsExtra_Id = 1}
            };

            var gesCaseReportsIndividuals = new List<I_GesCaseReportsG_Individuals>
            {
                new I_GesCaseReportsG_Individuals {I_GesCaseReports_Id = 1, G_Individuals_Id = 1}
            };

            var conventions = new List<I_Conventions>
            {
                new I_Conventions{I_Conventions_Id = 1}
            };

            var gesCaseReportsConventions = new List<I_GesCaseReportsI_Conventions>
            {
                new I_GesCaseReportsI_Conventions{I_GesCaseReports_Id = 1, I_Conventions_Id = 1, I_GesCaseReportsI_Conventions_Id = 1}
            };

            var norms = new List<I_Norms>
            {
                new I_Norms{I_Norms_Id = 1}    
            };

            var gesCaseReportsNorms = new List<I_GesCaseReportsI_Norms>
            {
                new I_GesCaseReportsI_Norms{I_GesCaseReports_Id = 1, I_Norms_Id = 1}
            };

            var managedDocuments = new List<G_ManagedDocuments>
            {
                new G_ManagedDocuments{G_ManagedDocuments_Id = 1}
            };

            var gesSourceDialogues = new List<I_GesSourceDialogues>
            {
                new I_GesSourceDialogues{I_GesSourceDialogues_Id = 1, I_GesCaseReports_Id = 1, G_Individuals_Id = 1, G_ManagedDocuments_Id = 1, I_ContactTypes_Id = 1}
            };

            var contactTypes = new List<I_ContactTypes>
            {
                new I_ContactTypes{I_ContactTypes_Id = 1}
            };

            var organizations = new List<G_Organizations>
            {
                new G_Organizations{G_Organizations_Id = 1}
            };

            GesEntities.Setup(c => c.Set<I_Companies>()).Returns(new MockDbSet<I_Companies>(companyData).Object);
            GesEntities.Setup(c => c.Set<I_GesCaseReports>()).Returns(new MockDbSet<I_GesCaseReports>(caseReportData).Object);
            GesEntities.Setup(c => c.Set<I_CalenderEvents>()).Returns(new MockDbSet<I_CalenderEvents>(calendarEvents).Object);
            GesEntities.Setup(c => c.Set<I_CalenderEvents>()).Returns(new MockDbSet<I_CalenderEvents>(calendarEvents).Object);
            GesEntities.Setup(c => c.Set<I_Conventions>()).Returns(new MockDbSet<I_Conventions>(conventions).Object);
            GesEntities.Setup(c => c.Set<I_GesCaseReportsI_Conventions>()).Returns(new MockDbSet<I_GesCaseReportsI_Conventions>(gesCaseReportsConventions).Object);
            GesEntities.Setup(c => c.Set<I_Norms>()).Returns(new MockDbSet<I_Norms>(norms).Object);
            GesEntities.Setup(c => c.Set<I_GesCaseReportsI_Norms>()).Returns(new MockDbSet<I_GesCaseReportsI_Norms>(gesCaseReportsNorms).Object);

            GesEntities.Setup(c => c.I_Companies).Returns(new MockDbSet<I_Companies>(companyData).Object);
            GesEntities.Setup(c => c.I_GesCompanies).Returns(new MockDbSet<I_GesCompanies>(gesCompanyData).Object);
            GesEntities.Setup(c => c.I_GesCaseReports).Returns(new MockDbSet<I_GesCaseReports>(caseReportData).Object);
            GesEntities.Setup(c => c.Countries).Returns(new MockDbSet<Countries>(countryData).Object);
            GesEntities.Setup(c => c.SubPeerGroups).Returns(new MockDbSet<SubPeerGroups>(msciData).Object);
            GesEntities.Setup(c => c.I_NormAreas).Returns(new MockDbSet<I_NormAreas>(normAreadData).Object);
            GesEntities.Setup(c => c.I_GesCommentary).Returns(new MockDbSet<I_GesCommentary>(commentaryData).Object);
            GesEntities.Setup(c => c.I_GesCaseReportStatuses).Returns(new MockDbSet<I_GesCaseReportStatuses>(recommendationData).Object);
            GesEntities.Setup(c => c.I_GesCaseReportsI_EngagementTypes).Returns(new MockDbSet<I_GesCaseReportsI_EngagementTypes>(engagementTypeData).Object);
            GesEntities.Setup(c => c.G_Services).Returns(new MockDbSet<G_Services>(serviceData).Object);
            GesEntities.Setup(c => c.G_OrganizationsG_Services).Returns(new MockDbSet<G_OrganizationsG_Services>(organizationsService).Object);
            GesEntities.Setup(c => c.I_GesCompanyDialogues).Returns(new MockDbSet<I_GesCompanyDialogues>(dialogueData).Object);
            GesEntities.Setup(c => c.I_GesLatestNews).Returns(new MockDbSet<I_GesLatestNews>(latestNewsData).Object);
            GesEntities.Setup(c => c.G_Users).Returns(new MockDbSet<G_Users>(usersData).Object);
            GesEntities.Setup(c => c.G_Individuals).Returns(new MockDbSet<G_Individuals>(individualData).Object);
            GesEntities.Setup(c => c.I_CompaniesG_ManagedDocuments).Returns(new MockDbSet<I_CompaniesG_ManagedDocuments>(manageDocumentData).Object);
            GesEntities.Setup(c => c.G_Uploads).Returns(new MockDbSet<G_Uploads>(uploadData).Object);
            GesEntities.Setup(c => c.GesCaseReportSignUp).Returns(new MockDbSet<GesCaseReportSignUp>(gesCaseReportSignUpData).Object);
            GesEntities.Setup(c => c.I_EngagementDiscussionPoints).Returns(new MockDbSet<I_EngagementDiscussionPoints>(engagementDiscussionPoints).Object);
            GesEntities.Setup(c => c.I_EngagementOtherStakeholderViews).Returns(new MockDbSet<I_EngagementOtherStakeholderViews>(engagementOtherStakeholderViews).Object);
            GesEntities.Setup(c => c.I_EngagementProfiles).Returns(new MockDbSet<I_EngagementProfiles>(engagementProfiles).Object);
            GesEntities.Setup(c => c.I_CalenderEvents).Returns(new MockDbSet<I_CalenderEvents>(calendarEvents).Object);
            GesEntities.Setup(c => c.I_ResponseStatuses).Returns(new MockDbSet<I_ResponseStatuses>(responseStatuses).Object);
            GesEntities.Setup(c => c.I_ProgressStatuses).Returns(new MockDbSet<I_ProgressStatuses>(progressStatuses).Object);
            GesEntities.Setup(c => c.I_EngagementTypes).Returns(new MockDbSet<I_EngagementTypes>(engagementTypes).Object);
            GesEntities.Setup(c => c.I_EngagementTypeCategories).Returns(new MockDbSet<I_EngagementTypeCategories>(engagementTypeCategories).Object);
            GesEntities.Setup(c => c.I_PortfoliosG_Organizations).Returns(new MockDbSet<I_PortfoliosG_Organizations>(portfoliosOrganizations).Object);
            GesEntities.Setup(c => c.I_PortfoliosG_OrganizationsG_Services).Returns(new MockDbSet<I_PortfoliosG_OrganizationsG_Services>(portfoliosOrganizationsServices).Object);
            GesEntities.Setup(c => c.I_PortfoliosI_Companies).Returns(new MockDbSet<I_PortfoliosI_Companies>(portfoliosCompanies).Object);
            GesEntities.Setup(c => c.G_OrganizationsG_Services).Returns(new MockDbSet<G_OrganizationsG_Services>(organizationsServices).Object);
            GesEntities.Setup(c => c.I_GesCaseReportsI_ProcessStatuses).Returns(new MockDbSet<I_GesCaseReportsI_ProcessStatuses>(gesCaseReportsProcessStatuses).Object);
            GesEntities.Setup(c => c.I_Milestones).Returns(new MockDbSet<I_Milestones>(milestones).Object);
            GesEntities.Setup(c => c.I_GesCaseReportsExtra).Returns(new MockDbSet<I_GesCaseReportsExtra>(gesCaseReportsExtra).Object);
            GesEntities.Setup(c => c.I_GesCaseReportsG_Individuals).Returns(new MockDbSet<I_GesCaseReportsG_Individuals>(gesCaseReportsIndividuals).Object);
            GesEntities.Setup(c => c.G_ManagedDocuments).Returns(new MockDbSet<G_ManagedDocuments>(managedDocuments).Object);
            GesEntities.Setup(c => c.I_GesSourceDialogues).Returns(new MockDbSet<I_GesSourceDialogues>(gesSourceDialogues).Object);
            GesEntities.Setup(c => c.I_ContactTypes).Returns(new MockDbSet<I_ContactTypes>(contactTypes).Object);
            GesEntities.Setup(c => c.G_Organizations).Returns(new MockDbSet<G_Organizations>(organizations).Object);
        }

        private void PrepareStoredProcedureData()
        {
            var companyListViewModel = new List<CompanyListViewModel>
            {
                new CompanyListViewModel {Id = 1, CompanyName = "Company 1", HomeCountry = "Sweden", CompanyId = 1},
                new CompanyListViewModel {Id = 2, CompanyName = "Company 2", HomeCountry = "Sweden", CompanyId = 2},
                new CompanyListViewModel {Id = 3, CompanyName = "Company 3", HomeCountry = "Sweden", CompanyId = 3}
            };

            StoredProcedureRunner
                .Setup(c => c.Execute<CompanyListViewModel>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>(),
                    It.IsAny<string[]>())).Returns(companyListViewModel);
        }
    }
}
