using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Inside.Data.DataContexts;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models.Anonymous;

namespace GES.Inside.Data.Services
{
    public class GesRolesService: EntityService<GesRefreshDbContext, GesRole>, IGesRolesService
    {
        private readonly GesRefreshDbContext _dbContext;
        private readonly IGesRolesRepository _gesURolesRepository;

        public GesRolesService(IUnitOfWork<GesRefreshDbContext> unitOfWork, IGesRolesRepository gesURolesRepository, IGesLogger logger)
            : base(unitOfWork, logger, gesURolesRepository)
        {
            _dbContext = unitOfWork.DbContext;
            _gesURolesRepository = gesURolesRepository;
        }

        public GesRole GetById(string id)
        {
            return this.SafeExecute<GesRole>(() => _gesURolesRepository.FindBy(x => x.Id == id).FirstOrDefault());
        }

        public PaginatedResults<GesRole> GetGesRoles(JqGridViewModel jqGridParams)
        {
            var query = from c in _dbContext.Roles select c;

            //SORT 
            query = query.OrderBy(x => x.Name);
             
            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesRole>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public IEnumerable<GesRole> GetGesRoles()
        {
            var query = from c in _dbContext.Roles select c;
            query = query.OrderBy(x => x.Name);

            return this.SafeExecute<IEnumerable<GesRole>>(() => query.ToList());
        }

        public IList<KeyValueObject<string, string>> GetRolesInString()
        {
            var listGesRoles = GetGesRoles();

            return this.SafeExecute(() => listGesRoles.Select(role => new KeyValueObject<string, string>(role.Id, role.Name)).ToList());
        }

        public string[] RolesToStringArray(ICollection<GesUserRole> roles)
        {
            var listGesRoles = GetGesRoles();
            var result = new List<string>();
            foreach (var role in roles)
            {
                var match = listGesRoles.FirstOrDefault(i => i.Id == role.RoleId);
                if (match != null)
                {
                    result.Add(match.Name);
                }
            }

            return result.ToArray();
        }

        public PaginatedResults<RoleModel> GetAllRolesForGrid(JqGridViewModel jqGridParams)
        {
            var query = from r in _dbContext.Roles
                        select new RoleModel
                        {
                            Id = r.Id,
                            Name = r.Name,
                            Accounts = r.Users.Count
                        };

            query = query.OrderBy(x => x.Name);

            //SORT
            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "name":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Name)
                            : query.OrderByDescending(x => x.Name);
                        break;
                    default:
                        break;
                }
            }

            if (!jqGridParams._search) return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
            var finalRules = JqGridHelper.GetFilterRules<RoleModel>(jqGridParams);
            query = string.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);

            return query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);
        }

        public List<RolePermissionModel> GetRolePermissions(string roleId)
        {
            var query = from f in _dbContext.GesForm
                        orderby f.SortOrder
                select new RolePermissionModel()
                {
                    FormName = f.Name,
                    RoleId = roleId,
                    AllowedAction = 1,
                    FormId = f.Id,
                    AllowedRead = (from rp in _dbContext.GesRolePermission where rp.GesFormId == f.Id && rp.GesRoleId == roleId select rp.Id).Any()
                };

            return query.ToList();
        }

        public List<GesAccountRoleViewModel> GetUsersInRole(string roleId)
        {
            var query = from u in _dbContext.Users
                        join ur in _dbContext.UserRoles on u.Id equals  ur.UserId

                        where ur.RoleId == roleId
                select new GesAccountRoleViewModel()
                {
                    Id = u.Id,
                    OldUserId = u.OldUserId,
                    UserName = u.UserName,
                    Email = u.Email
                };

            return query.ToList();
        }

        public List<GesAccountRoleViewModel> GetUsersNotInRole(string roleId)
        {
            var accessInsideClaim = ClaimEnum.AccessInside.GetEnumDescription();

            var query = from u in _dbContext.Users
                where !(from ur in _dbContext.UserRoles where ur.RoleId == roleId select ur.UserId).Contains(u.Id)
                        && u.Claims.Any(d=>d.ClaimType == accessInsideClaim)
                select new GesAccountRoleViewModel()
                {
                    Id = u.Id,
                    OldUserId = u.OldUserId,
                    UserName = u.UserName,
                    Email = u.Email
                };

            return query.ToList();
        }

    }
}
