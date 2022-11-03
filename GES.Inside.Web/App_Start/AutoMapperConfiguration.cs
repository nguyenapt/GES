using AutoMapper;
using GES.Inside.Data.Configs;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models;
using GES.Inside.Web.Models;

namespace GES.Inside.Web
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            var mappings = AutoMapperDataConfiguration.Configurations();
            mappings.CreateMap<I_GesCaseReports, BasicCaseProfileViewModel>();
            mappings.CreateMap<DocumentViewModel, G_ManagedDocuments>();
            Mapper.Initialize(mappings);
        }
    }
}