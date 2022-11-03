using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class GesUserRolesService: EntityService<GesRefreshDbContext, GesUserRole>, IGesUserRolesService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IGesUserRolesRepository _gesUserRolesRepository;

        public GesUserRolesService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IGesUserRolesRepository gesUserRolesRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesUserRolesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesUserRolesRepository = gesUserRolesRepository;
        }

        public List<GesUserRole> GetByUserId(string userId)
        {
            return this.SafeExecute<List<GesUserRole>>(() => _gesUserRolesRepository.GetByUserId(userId));
        }
    }
}
