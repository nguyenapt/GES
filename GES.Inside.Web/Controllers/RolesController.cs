using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using AutoMapper;
using GES.Common.Enumeration;
using GES.Common.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;
using GES.Inside.Data.Models;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Repository.Interfaces;

namespace GES.Inside.Web.Controllers
{
    public class RolesController : GesControllerBase
    {
        #region Declaration

        private readonly IGesRolesService _rolesService;
        private readonly IGesRolesRepository _rolesRepository;
        private readonly IGesUserRolesRepository _userRolesRepository;
        private readonly IGesRolePermissionRepository _rolePermissionRepository;
        private readonly IOldUserService _oldUserService;

        #endregion

        #region Constructor

        public RolesController(IGesLogger logger, IGesRolesService rolesService, IGesRolesRepository rolesRepository, IGesRolePermissionRepository rolePermissionRepository, IOldUserService oldUserService, IGesUserRolesRepository userRolesRepository
        )
            : base(logger)
        {
            _rolesService = rolesService;
            _rolesRepository = rolesRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _oldUserService = oldUserService;
            _userRolesRepository = userRolesRepository;
        }

        #endregion

        #region ActionResult

        [CustomAuthorize(FormKey = "ConfigAccountRole", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            this.SafeExecute(() =>
            {

            }, $"Error when getting the viewbag information. Please check inner exception for detail.");

            ViewBag.Title = "Accounts Roles";

            return View();
        }

        [CustomAuthorize(FormKey = "ConfigAccountRole", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            ViewBag.RoleId = id;

            var roleName = "Add new Role";
            if (!string.IsNullOrEmpty(id))
            {
                var role = this.SafeExecute(() => _rolesService.GetById(id), null);
                if (role != null)
                {
                    roleName = role.Name;
                }
            }

            ViewBag.Title = $"Role: {roleName}";
            ViewBag.NgController = "RoleController";

            return View();
        }

        public ActionResult Add()
        {
            ViewBag.NgController = "RoleController";
            return View("Details");
        }

        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForRolesJqGrid(JqGridViewModel jqGridParams)
        {
            var listRoles = this.SafeExecute(() => _rolesService.GetAllRolesForGrid(jqGridParams),
                "Error when getting the clients with criteria {@JqGridViewModel}", jqGridParams);

            return Json(listRoles);
        }

        [HttpGet]
        public JsonResult GetRolePermissions(string roleId)
        {
            var listRoles = this.SafeExecute(() => _rolesService.GetRolePermissions(roleId),
                $"Error when getting the permission with roleId: {roleId}");

            ViewBag.NgController = "RoleController";
            return Json(listRoles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsersInRole(string roleId)
        {
            var listRoles = this.SafeExecute(() => _rolesService.GetUsersInRole(roleId),
                $"Error when getting the permission with roleId: {roleId}");

            var listOldUsers = _oldUserService.GetListOldUsers();

            var result = (from q in listRoles
                         join o in listOldUsers on q.OldUserId equals o.G_Users_Id
                select new GesAccountRoleViewModel
                {
                    Id = q.Id,
                    UserName = q.UserName,
                    Email = q.Email,
                    OldUserId = q.OldUserId,
                    FirstName = o.G_Individuals1?.FirstName ?? "",
                    LastName = o.G_Individuals1?.LastName ?? "",
                    OrgName = o.G_Individuals1?.G_Organizations?.Name ?? "",
                    RoleId = roleId
                }).ToList();

            ViewBag.NgController = "RoleController";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRoleDetails(string id)
        {
            RoleModel normModel = null;
            if (!string.IsNullOrEmpty(id))
            {
                var roleDetails = _rolesService.GetById(id);
                normModel = Mapper.Map<RoleModel>(roleDetails);
            }

            ViewBag.NgController = "RoleController";
            return Json(normModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateRole(RoleModel roleModel, List<RolePermissionModel> permissionModels)
        {
            try
            {
                GesRole updatingRole;
                if (roleModel.Id == null)
                {
                    updatingRole = new GesRole();
                }
                else
                {
                    updatingRole = _rolesRepository.GetById(roleModel.Id);
                    if (updatingRole == null)
                        return null;
                }

                updatingRole.Name = roleModel.Name;


                if (roleModel.Id == null)
                {
                    updatingRole.Id = Guid.NewGuid().ToString();
                    _rolesRepository.Add(updatingRole);
                }
                else
                {
                    _rolesRepository.Edit(updatingRole);
                }
                
                _rolesRepository.Save();

                var listPermissions = _rolePermissionRepository.GetPermissionsByRoleId(updatingRole.Id);

                foreach (var permission in listPermissions)
                {
                    _rolePermissionRepository.Delete(permission);
                }

                foreach (var item in permissionModels)
                {
                    if (item.AllowedRead)
                    {
                        _rolePermissionRepository.Add(new GesRolePermission()
                        {
                            GesRoleId = updatingRole.Id,
                            GesFormId = item.FormId,
                            AllowedAction = 1
                        });
                    }
                }

                _rolePermissionRepository.Save();

                return Json(new { Status = "Success" });
            }
            catch (Exception e)
            {
                return Json(new { Status = "Save Failed, Caused: " + e.Message});
            }
        }
        
        [HttpPost]
        public JsonResult DeleteRole(string roleId)
        {
            try
            {
                var guiderline = _rolesRepository.GetById(roleId);

                if (guiderline != null)
                {
                    _rolesRepository.Delete(guiderline);

                    _rolesRepository.Save();
                }
            }
            catch (Exception e)
            {
                return Json(new { Status = "Error, Caused:" + e.Message });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult DeleteUserRole(GesAccountRoleViewModel userRole)
        {
            try
            {
                var gesUserRole = _userRolesRepository.GetByUserAndRoleId(userRole.Id, userRole.RoleId);

                if (gesUserRole != null)
                {
                    _userRolesRepository.Delete(gesUserRole);
                    _userRolesRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult AddUserToRole(string userId, string roleId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(roleId))
                {
                    _userRolesRepository.Add(new GesUserRole(){RoleId = roleId, UserId = userId});
                    _userRolesRepository.Save();
                }
            }
            catch
            {
                return Json(new { Status = "Error" });
            }
            return Json(new { Status = "Success" });
        }

        [HttpPost]
        public JsonResult GetAllUserForRoleJqGrid(JqGridViewModel jqGridParams, string roleId)
        {
            var listRoles = _rolesService.GetUsersNotInRole(roleId);

            var listOldUsers = _oldUserService.GetListOldUsers();

            var query = (from q in listRoles
                join o in listOldUsers on q.OldUserId equals o.G_Users_Id
                select new GesAccountRoleViewModel
                {
                    Id = q.Id,
                    UserName = q.UserName,
                    Email = q.Email,
                    OldUserId = q.OldUserId,
                    FirstName = o.G_Individuals1?.FirstName ?? "",
                    LastName = o.G_Individuals1?.LastName ?? "",
                    OrgName = o.G_Individuals1?.G_Organizations?.Name ?? "",
                    RoleId = roleId
                });

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
                    case "firstname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.FirstName)
                            : query.OrderByDescending(x => x.FirstName);
                        break;
                    case "lastname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.LastName)
                            : query.OrderByDescending(x => x.LastName);
                        break;
                    case "email":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.Email)
                            : query.OrderByDescending(x => x.Email);
                        break;
                    case "orgname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.OrgName)
                            : query.OrderByDescending(x => x.OrgName);
                        break;
                    default:
                        break;
                }
            }

            if (jqGridParams._search)
            {
                var finalRules = JqGridHelper.GetFilterRules<GesAccountRoleViewModel>(jqGridParams);
                query = String.IsNullOrEmpty(finalRules) ? query : query.Where(finalRules);
            }

            var result = query.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows);


            return Json(result);
        }

        #endregion

        #region Private methods

        #endregion

    }
}