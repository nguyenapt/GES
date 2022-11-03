using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Claims;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using Z.EntityFramework.Plus;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Data.Services
{
    public class GesUserService: EntityService<GesRefreshDbContext, GesUser>, IGesUserService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IGesUserRepository _gesUserRepository;

        public GesUserService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IGesUserRepository gesUserRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesUserRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesUserRepository = gesUserRepository;
        }

        public GesUser GetById(string id)
        {
            return this.SafeExecute<GesUser>(() => _gesUserRepository.FindBy(x => x.Id == id).FirstOrDefault());
        }

        public GesUser GetByOldUserId(long id)
        {
            return this.SafeExecute<GesUser>(() => _gesUserRepository.FindBy(x => x.OldUserId == id).FirstOrDefault());
        }

        public PaginatedResults<GesAccountViewModel> GetGesUsers(JqGridViewModel jqGridParams)
        {
            Common.Exceptions.Guard.AgainstNullArgument(nameof(jqGridParams), jqGridParams);

            var query = this._gesUserRepository.GetAllAccounts();

            //SORT 
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "username":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.UserName)
                            : query.OrderByDescending(x => x.UserName);
                        break;
                    case "email":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Email)
                            : query.OrderByDescending(x => x.Email);
                        break;
                    case "rolelist":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.RoleList.Count()).ThenBy(x => x.UserName)
                            : query.OrderByDescending(x => x.RoleList.Count()).ThenBy(x => x.UserName);
                        break;
                    default:
                        query = query.OrderByDescending(x => x.RoleList.Count()).ThenBy(x => x.UserName);
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesAccountViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }


        public List<GesAccountViewModel> GetAllGesUsers()
        {
            var query = this._gesUserRepository.GetAllAccountsWithLockedInformation().AsQueryable().FromCache("AllGesUsers");

            return query.ToList();
        }
        
        public List<long> GetOldUserIdLastLogin(double hours)
        {
            var timeThreshold = DateTime.Now.AddHours(hours);
            return this.SafeExecute<List<long>>(() => _gesUserRepository.FindBy(x => x.OldUserId != null && x.LastLogIn != null && x.LastLogIn.Value > timeThreshold)
                    .Select(d => d.OldUserId.Value)
                    .ToList());
        }

        public GesUser GetByUserName(string userName)
        {
            return this.SafeExecute<GesUser>(() => _gesUserRepository.FindBy(x => x.UserName.Equals(userName)).FirstOrDefault());
        }

    }
}
