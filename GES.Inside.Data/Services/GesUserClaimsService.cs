using System.Collections.Generic;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Services
{
    public class GesUserClaimsService: EntityService<GesRefreshDbContext, GesUserClaim>, IGesUserClaimsService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IGesUserClaimsRepository _gesUserClaimsRepository;

        public GesUserClaimsService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IGesUserClaimsRepository gesUserClaimsRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesUserClaimsRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesUserClaimsRepository = gesUserClaimsRepository;
        }

        public List<GesUserClaim> GetByUserId(string userId)
        {
            return this.SafeExecute<List<GesUserClaim>>(() => _gesUserClaimsRepository.GetByUserId(userId));
        }
    }
}
