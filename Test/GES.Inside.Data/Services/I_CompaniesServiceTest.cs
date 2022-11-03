using System.Linq;
using FluentAssertions;
using GES.Common.Models;
using GES.Inside.Data.Services;
using NUnit.Framework;
using Test.Common;

namespace Test.GES.Inside.Data.Services
{
    class I_CompaniesServiceTest : TestBase
    {
        private I_CompaniesService _service;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _service = new I_CompaniesService(DbContextGesEntities, CompaniesRepository, GesCaseReportsRepository, null, Logger.Object);
        }

        [TestCase("Company", 3)]
        [TestCase("Company 1", 1)]
        public void GetCompaniesWithTerm(string keyword, int expectedCompanyCount)
        {
            var result = _service.GetCompaniesWithTerm(keyword, 30);
            result.Count.Should().Be(expectedCompanyCount);
            result[0].Id.Should().Be(1);
            result[0].Name.Should().Be("#1. Company 1");
        }

        [TestCase("Company", 3)]
        [TestCase("Company 1", 1)]
        public void GetCompaniesAndSubCompaniesWithTerm(string keyword, int expectedCompanyCount)
        {
            var result = _service.GetCompaniesAndSubCompaniesWithTerm(keyword, 30);

            result.Count.Should().Be(expectedCompanyCount);
            result[0].Key.Should().Be("Isin1");
            result[0].Value.Should().Be("#1. Company 1");
        }

        [TestCase(2, "Isin1", "Isin2")]
        [TestCase(3, "Isin1", "Isin2", "Isin3")]
        public void GetCompaniesByListIsin(int expectedCompanyCount, params string[] isinCodes)
        {
            var result = _service.GetCompaniesByListIsin(isinCodes).ToList();
            result.Count.Should().Be(expectedCompanyCount);
            result[0].Isin.Should().Be("Isin1");
            result[1].Isin.Should().Be("Isin2");
        }

        [TestCase(2, 1, 2)]
        [TestCase(3, 1, 2, 3)]
        [TestCase(2, 1, 2, 4, 5, 6)]
        public void GetCompaniesByListIds(int expectedCompanyCount, params long[] companyIds)
        {
            var result = _service.GetCompaniesByListIds(companyIds).ToList();
            result.Count.Should().Be(expectedCompanyCount);
            result[0].I_Companies_Id.Should().Be(1);
            result[1].I_Companies_Id.Should().Be(2);
        }

        [Test]
        public void GetCompaniesWithPaging()
        {
            var result = _service.GetCompanies(new JqGridViewModel { page = 1, rows = 100 });
            result.Results.Count().Should().Be(3);
        }
    }
}
