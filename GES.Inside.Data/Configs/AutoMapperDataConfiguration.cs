using AutoMapper;
using AutoMapper.Configuration;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.CaseProfiles;

namespace GES.Inside.Data.Configs
{
    public class AutoMapperDataConfiguration
    {
        public static MapperConfigurationExpression Configurations()
        {
            var mappings = new MapperConfigurationExpression { CreateMissingTypeMaps = true };

            //CaseProfileBaseComponent
            mappings.CreateMap<CaseProfileBaseComponent, SrEmeCaseProfileBaseComponent>();

            //CaseProfileIssueComponent
            mappings.CreateMap<CaseProfileIssueComponent, BusinessConductEngageOrDisEngageOrResolveCaseProfileIssueComponent>();
            mappings.CreateMap<CaseProfileIssueComponent, SrEmeCaseProfileIssueComponent>();

            //CaseProfileCaseComponent
            mappings.CreateMap<CaseProfileCaseComponent, BcEngageCaseProfileCaseComponent>();
            mappings.CreateMap<CaseProfileCaseComponent, BcDisEngageCaseProfileCaseComponent>();
            mappings.CreateMap<CaseProfileCaseComponent, BcEvaluateCaseProfileCaseComponent>();
            mappings.CreateMap<CaseProfileCaseComponent, SrCaseProfileCaseComponent>();

            mappings.CreateMap<DocumentViewModel, G_ManagedDocuments>();
            mappings.CreateMap<CaseProfileBcDisengageViewModel, CaseProfileBcResolvedViewModel>();
            return mappings;
        }

        public static void Configure()
        {
            Mapper.Initialize(Configurations());
        }
    }
}
