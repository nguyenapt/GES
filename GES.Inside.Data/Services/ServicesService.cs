using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Common.Models;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;

namespace GES.Inside.Data.Services
{
    public class ServicesService : EntityService<GesEntities, G_Services>, II_ServicesService
    {
        private readonly IG_ServicesRepository _servicesRepository;

        public ServicesService(IUnitOfWork<GesEntities> unitOfWork, IG_ServicesRepository servicesRepository, IGesLogger logger)
            : base(unitOfWork, logger, servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public G_Services GetById(long id)
        {
            return this.SafeExecute(() => _servicesRepository.GetById(id));
        }

        public PaginatedResults<ServicesModel> GetGesServices(JqGridViewModel jqGridParams)
        {
            var services = _servicesRepository.GetGesServices();

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<ServicesModel>(jqGridParams);
                services = string.IsNullOrEmpty(finalRules) ? services : services.Where(finalRules);
            }

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir))
                return services.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);

            switch (sortCol)
            {
                case "name":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.Name)
                        : services.OrderByDescending(x => x.Name);
                    break;
                case "sort":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.Sort).ThenBy(d => d.Name)
                        : services.OrderByDescending(x => x.Sort).ThenBy(d => d.Name);
                    break;
                case "url":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.Url).ThenBy(d => d.Name)
                        : services.OrderByDescending(x => x.Url).ThenBy(d => d.Name);
                    break;
                case "showinnavigation":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.ShowInNavigation).ThenBy(d => d.Name)
                        : services.OrderByDescending(x => x.ShowInNavigation).ThenBy(d => d.Name);
                    break;
                case "reportletter":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.ReportLetter).ThenBy(d => d.Name)
                        : services.OrderByDescending(x => x.ReportLetter).ThenBy(d => d.Name);
                    break;
                case "engagementtypesname":
                    services = sortDir == "asc"
                        ? services.OrderBy(x => x.EngagementTypesName).ThenBy(d => d.Name)
                        : services.OrderByDescending(x => x.EngagementTypesName).ThenBy(d => d.Name);
                    break;
            }

            return services.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);

        }

        public IEnumerable<ServicesModel> GetGesServices()
        {
            return _servicesRepository.GetGesServices();
        }
    }
}
