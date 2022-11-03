using FluentAssertions;
using GES.Inside.Data.Services;
using NUnit.Framework;
using Test.Common;

namespace Test.GES.Inside.Data.Services
{
    class I_GesCaseProfileServiceTest : TestBase
    {
        private I_GesCaseProfilesService _service;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _service = new I_GesCaseProfilesService(DbContextGesEntities, GesCaseReportsRepository, CompaniesRepository, GesCaseReportsExtraRepository, ManagedDocumentsRepository, CaseReportSignUpRepository, null, CalendarService, Logger.Object);
        }

        class BusinessConductEngageTest : I_GesCaseProfileServiceTest
        {
            [Test]
            public void GetBaseComponent()
            {
                var result = _service.GetBusinessConductEngage(1, 1);
                result.Should().NotBeNull();

                var baseComponent = result.BaseComponent;
                baseComponent.Should().NotBeNull();
                baseComponent.CaseProfileId.Should().Be(1);
                baseComponent.CompanyName.Should().Be("Company 1");
                baseComponent.CompanyIsin.Should().Be("Isin1");
            }

            [Test]
            public void GetCaseComponent()
            {
                var result = _service.GetBusinessConductEngage(1, 1);
                result.Should().NotBeNull();

                var caseComponent = result.CaseComponent;
                caseComponent.Should().NotBeNull();
                caseComponent.Norm.Should().Be("Business Conduct - Environment");
                caseComponent.CaseProfileId.Should().Be(1);
                caseComponent.Location.Should().Be("Sweden");
            }

            [Test]
            public void GetIssueComponent()
            {
                var result = _service.GetBusinessConductEngage(1, 1);
                result.Should().NotBeNull();

                var issueComponent = result.IssueComponent;
                issueComponent.Should().NotBeNull();
                issueComponent.CaseProfileId.Should().Be(1);
                issueComponent.LatestNews.Should().Be("Latest new content");
            }

            [Test]
            public void GetStatisticComponent()
            {
                var result = _service.GetBusinessConductEngage(1, 1);
                result.Should().NotBeNull();

                var statisticComponent = result.StatisticComponent;
                statisticComponent.Should().NotBeNull();
                statisticComponent.ConferenceCount.Should().Be(0);
            }

            [Test]
            public void GetContactEngagementManager()
            {
                var result = _service.GetBusinessConductEngage(1, 1);
                result.Should().NotBeNull();

                var contactInfo = result.ContactEngagementManager;
                contactInfo.Should().NotBeNull();
                contactInfo.FirstName.Should().Be("Hanna");
                contactInfo.LastName.Should().Be("Robert");
                contactInfo.Email.Should().Be("hanna.robert@ges.com");
                contactInfo.JobTitle.Should().Be("Product Owner");
            }

            [Test]
            public void GetAdditionalIncident()
            {
                var result = _service.GetAdditionalCaseReports(1, 1);
                result.Should().NotBeNull();
                result[0].Should().NotBeNull();
                result[0].Id.Should().Be(2);
                Assert.That(result, Is.Ordered.By("SortOrderEngagementType").Then.By("SortOrderRecommendation").Descending);
            }
        }

        class BusinessConductEvaluateTest : I_GesCaseProfileServiceTest
        {
            [Test]
            public void GetBaseComponent()
            {
                var result = _service.GetBusinessConductEvaluate(1, 1);
                result.Should().NotBeNull();

                var baseComponent = result.BaseComponent;
                baseComponent.Should().NotBeNull();
                baseComponent.CaseProfileId.Should().Be(1);
                baseComponent.CompanyName.Should().Be("Company 1");
                baseComponent.CompanyIsin.Should().Be("Isin1");
            }

            [Test]
            public void GetCaseComponent()
            {
                var result = _service.GetBusinessConductEvaluate(1, 1);
                result.Should().NotBeNull();

                var caseComponent = result.CaseComponent;
                caseComponent.Should().NotBeNull();
                caseComponent.Norm.Should().Be("Business Conduct - Environment");
                caseComponent.CaseProfileId.Should().Be(1);
                caseComponent.Location.Should().Be("Sweden");
            }
        }
    }
}
