using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Models;
using System.Collections.Generic;
using GES.Common.Enumeration;

namespace GES.Inside.Data.Services
{
    public class G_IndividualsService : EntityService<GesEntities, G_Individuals>, IG_IndividualsService
    {
        private readonly GesEntities _dbContext;
        private readonly IG_IndividualsRepository _gIndividualsRepository;
        private readonly IG_OrganizationsG_ServicesRepository _organizationServiceRepository;

        public G_IndividualsService(IUnitOfWork<GesEntities> unitOfWork, IG_IndividualsRepository gIndividualsRepository, IGesLogger logger,
            IG_OrganizationsG_ServicesRepository organizationsG_ServicesRepository)
            : base(unitOfWork, logger, gIndividualsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gIndividualsRepository = gIndividualsRepository;
            _organizationServiceRepository = organizationsG_ServicesRepository;
        }

        public G_Individuals GetIndividualByUserId(long userId)
        {
            return this.SafeExecute<G_Individuals>(() => this._gIndividualsRepository.GetIndividualByUser(userId));
        }

        public bool CheckServiceForIndividual(long individualId)
        {
            return this.SafeExecute<bool>(() =>
            {
                return this._organizationServiceRepository.GetSubscribedServicesOfIndividual(individualId).Any();
            });
        }

        public G_Individuals GetById(long gIndividualId)
        {
            return this.SafeExecute<G_Individuals>(() => _gIndividualsRepository.GetById(gIndividualId));
        }

        public IList<DialogueCaseModel> GetCompanyDialogueLogsByIndividual(long individualId)
        {
            var result = from dialogue in _dbContext.I_GesCompanyDialogues
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from contactDirection in _dbContext.I_ContactDirections.Where(contactDirection => contactDirection.I_ContactDirections_Id == dialogue.I_ContactDirections_Id).DefaultIfEmpty()
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id).DefaultIfEmpty()
                         from caseProfile in _dbContext.I_GesCaseReports.Where(c => c.I_GesCaseReports_Id == dialogue.I_GesCaseReports_Id).DefaultIfEmpty()
                         from gescompany in _dbContext.I_GesCompanies.Where(c => c.I_GesCompanies_Id == caseProfile.I_GesCompanies_Id).DefaultIfEmpty()
                         from company in _dbContext.I_Companies.Where(c => c.I_Companies_Id == gescompany.I_Companies_Id).DefaultIfEmpty()
                         join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on dialogue.I_GesCaseReports_Id equals cret.I_GesCaseReports_Id
                         join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into etg
                         from et in etg.DefaultIfEmpty()
                         join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                         from etc in etcg.DefaultIfEmpty()
                         join n in _dbContext.I_NormAreas on caseProfile.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                         from n in ng.DefaultIfEmpty()
                         where dialogue.G_Individuals_Id == individualId
                         orderby dialogue.ContactDate descending
                         select new DialogueCaseModel
                         {
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id,
                             ContactDirectionName = contactDirection.Name,
                             ContactTypeId = contactType.I_ContactTypes_Id,
                             ContactTypeName = contactType.Name,
                             CompanyId = company.I_Companies_Id,
                             I_GesCaseReports_Id = dialogue.I_GesCaseReports_Id,
                             CompanyName = company.Name,
                             Issue = caseProfile.ReportIncident,    
                             Norm = (!etc.Name.ToLower().Contains("conduct")
                                    ? etc.Name.Replace("Engagement", "")
                                    : et.Name.ToLower().Contains("extended") ? et.Name : "Business Conduct").Trim()
                                    + " - " + (cret.I_EngagementTypes_Id == (int)EngagementTypeEnum.Conventions ? n.Name : et.Name.Replace("Engagement", "")).Trim(),
                             DialogueType = "Company"
                         };
            return SafeExecute(() => result.ToList());
        }

        public IList<DialogueCaseModel> GetSourceDialogueLogsByIndividual(long individualId)
        {
            var result = from dialogue in _dbContext.I_GesSourceDialogues
                         from contactType in _dbContext.I_ContactTypes.Where(contactType => contactType.I_ContactTypes_Id == dialogue.I_ContactTypes_Id).DefaultIfEmpty()
                         from contactDirection in _dbContext.I_ContactDirections.Where(contactDirection => contactDirection.I_ContactDirections_Id == dialogue.I_ContactDirections_Id).DefaultIfEmpty()
                         from individual in _dbContext.G_Individuals.Where(individual => individual.G_Individuals_Id == dialogue.G_Individuals_Id).DefaultIfEmpty()
                         from caseProfile in _dbContext.I_GesCaseReports.Where(c => c.I_GesCaseReports_Id == dialogue.I_GesCaseReports_Id).DefaultIfEmpty()
                         from gescompany in _dbContext.I_GesCompanies.Where(c => c.I_GesCompanies_Id == caseProfile.I_GesCompanies_Id).DefaultIfEmpty()
                         from company in _dbContext.I_Companies.Where(c => c.I_Companies_Id == gescompany.I_Companies_Id).DefaultIfEmpty()
                         join cret in _dbContext.I_GesCaseReportsI_EngagementTypes on dialogue.I_GesCaseReports_Id equals cret.I_GesCaseReports_Id
                         join et in _dbContext.I_EngagementTypes on cret.I_EngagementTypes_Id equals et.I_EngagementTypes_Id into etg
                         from et in etg.DefaultIfEmpty()
                         join etc in _dbContext.I_EngagementTypeCategories on et.I_EngagementTypeCategories_Id equals etc.I_EngagementTypeCategories_Id into etcg
                         from etc in etcg.DefaultIfEmpty()
                         join n in _dbContext.I_NormAreas on caseProfile.I_NormAreas_Id equals n.I_NormAreas_Id into ng
                         from n in ng.DefaultIfEmpty()
                         where dialogue.G_Individuals_Id == individualId
                         orderby dialogue.ContactDate descending
                         select new DialogueCaseModel
                         {
                             ContactDate = dialogue.ContactDate,
                             ContactDirectionId = dialogue.I_ContactDirections_Id,
                             ContactDirectionName = contactDirection.Name,
                             ContactTypeId = contactType.I_ContactTypes_Id,
                             ContactTypeName = contactType.Name,
                             CompanyId = company.I_Companies_Id,
                             I_GesCaseReports_Id = dialogue.I_GesCaseReports_Id,
                             CompanyName = company.Name,
                             Issue = caseProfile.ReportIncident,
                             Norm = (!etc.Name.ToLower().Contains("conduct")
                                    ? etc.Name.Replace("Engagement", "")
                                    : et.Name.ToLower().Contains("extended") ? et.Name : "Business Conduct").Trim()
                                    + " - " + (cret.I_EngagementTypes_Id == (int)EngagementTypeEnum.Conventions ? n.Name : et.Name.Replace("Engagement", "")).Trim(),
                             DialogueType = "Source"
                         };
            return SafeExecute(() => result.ToList());
        }
    }
}
