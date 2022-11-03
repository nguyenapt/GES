using System.Linq;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Data.Repository
{
    public class GesRolesRepository : GenericRepository<GesRole>, IGesRolesRepository
    {
        private readonly GesRefreshDbContext _dbContext;

        public GesRolesRepository(GesRefreshDbContext context, IGesLogger logger)
           : base(context, logger)
        {
            _dbContext = context;
        }

        public GesRole GetById(string id)
        {
            return this.SafeExecute<GesRole>(() => _entities.Set<GesRole>().FirstOrDefault(d => d.Id == id));
        }

        public bool CheckPermission(string userId, string formKey, int action)
        {
            var query = from r in _dbContext.Roles
                join p in _dbContext.GesRolePermission on r.Id equals p.GesRoleId
                join f in _dbContext.GesForm on p.GesFormId equals f.Id
                join ur in _dbContext.UserRoles on r.Id equals ur.RoleId
                where p.AllowedAction == action && ur.UserId == userId && f.FormKey == formKey
                        select r.Id;

            return query.Any();
        }

    }
}
