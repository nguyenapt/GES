using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GesUserRepository : GenericRepository<GesUser>, IGesUserRepository
    {
        protected readonly GesRefreshDbContext _dbContext;

        public GesUserRepository(GesRefreshDbContext context, IGesLogger logger)
           : base(context, logger)
        {
            this._dbContext = context;
        }

        public IEnumerable<GesAccountViewModel> GetAllAccounts()
        {
            return from c in _dbContext.Users
                   select new GesAccountViewModel
                   {
                       Id = c.Id,
                       UserName = c.UserName,
                       Email = c.Email,
                       OldUserId = c.OldUserId,
                       RoleList = c.Roles.Select(d => d.RoleId)
                   };
        }

        public IEnumerable<GesAccountViewModel> GetAllAccountsWithLockedInformation()
        {
            var utcDateNow = DateTime.UtcNow;

            return from c in _dbContext.Users
                   select new GesAccountViewModel
                   {
                       Id = c.Id,
                       UserName = c.UserName,
                       Email = c.Email,
                       OldUserId = c.OldUserId,
                       IsLocked = c.LockoutEnabled && c.LockoutEndDateUtc != null && c.LockoutEndDateUtc > utcDateNow,
                       RoleList = c.Roles.Select(d => d.RoleId),
                       LastLogIn = c.LastLogIn,
                       Claims = c.Claims.Select(d=>d.ClaimType),
                       Password = c.PasswordHash
                   };
        }

        public GesUser GetById(string id)
        {
            return this.SafeExecute<GesUser>(() => _entities.Set<GesUser>().FirstOrDefault(d => d.Id == id));
        }
    }
}
