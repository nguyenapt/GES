using GES.Common.Logging;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GES.Inside.Data.Services
{
    public class I_CompaniesI_ManagementSystemsService : EntityService<GesEntities, I_CompaniesI_ManagementSystems>, II_CompaniesI_ManagementSystemsService
    {
        private readonly GesEntities _dbContext;
        private readonly II_CompaniesI_ManagementSystemsRepository _companiesI_ManagementSystemsRepository;

        public I_CompaniesI_ManagementSystemsService(IUnitOfWork<GesEntities> unitOfWork, II_CompaniesI_ManagementSystemsRepository companiesI_ManagementSystemsRepository, IGesLogger logger)
            : base(unitOfWork, logger, companiesI_ManagementSystemsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _companiesI_ManagementSystemsRepository = companiesI_ManagementSystemsRepository;
        }

        public I_CompaniesI_ManagementSystems GetById(long id)
        {
            return SafeExecute(() => _companiesI_ManagementSystemsRepository.FindBy(x=>x.I_CompaniesI_ManagementSystems_Id==id).FirstOrDefault());
        }

    }
}
