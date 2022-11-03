using NUnit.Framework;
using Sustainalytics.GSS.DataLayer;
using Sustainalytics.GSS.Entities;
using System.Linq;

namespace Test.GSS.DataLayer
{
    public class GssRepositoryTests
    {
        private const string gssConnectionString = "Data Source=(local);Initial Catalog=GSS;Integrated Security=True";
        private const string testUser = "TEST";
        
        [Test]
        public void ListCompanies()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var filteredCompaniesPageWithTotalCount = gssRepository.GetFilteredCompaniesPageWithCount(0, 50);
        }

        [Test]
        public void GetCompany()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var company = gssRepository.GetCompany(1007896757);
        }

        [Test]
        public void ListPermittedTransitions()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var permittedTransitions = gssRepository.ListPermittedTransitions(1007896757);
        }

        [Test]
        public void SetCompanyProfileStatus()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var permittedTransitions = gssRepository.ListPermittedTransitions(1007896757);

            gssRepository.SetCompanyProfileStatus(1007896757, permittedTransitions[0]);
        }

        [Test]
        public void GetGssQuarter()
        {
            var gssQuarter = new System.DateTime(2019, 2, 22).GetGssQuarter();// Q2 2019; it started on the last Friday of February
        }

        [Test]
        public void UpdateOutlook()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var company = gssRepository.GetCompany(1007896757);

            if (company.LatestProfile != null)
            {
                gssRepository.UpdateOutlook(company.LatestProfile.Id, FeedbackType.Negative, new SpecificQuarter { Numerator = Quarter.Three, Year = 2018 }, "Something happened on the way to heaven...");
            }
        }

        [Test]
        public void ListIssueIndicators()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var company = gssRepository.GetCompany(1007896757);

            if (company.LatestProfile != null)
            {
                var p1Indicators = gssRepository.ListIssueIndicators(company.LatestProfile.Principles[0].Id);
            }
        }

        [Test]
        public void GetIssueIndicator()
        {
            var gssRepository = new GssRepository(gssConnectionString, testUser);

            var company = gssRepository.GetCompany(1007896757);

            if (company.LatestProfile != null)
            {
                var p1Indicators = gssRepository.ListIssueIndicators(company.LatestProfile.Principles[0].Id);

                var issueIndicator = gssRepository.GetIssueIndicator(p1Indicators.First().Key);
            }
        }
    }
}
