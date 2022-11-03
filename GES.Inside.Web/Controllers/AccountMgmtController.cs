using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GES.Common.Configurations;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Common.Helpers;
using GES.Common.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using GES.Inside.Data.Helpers;

namespace GES.Inside.Web.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class AccountMgmtController : GesControllerBase
    {
        #region Declaration
        private readonly IGesUserService _gesUserService;
        private readonly IGesUserRolesService _gesUserRolesService;
        private IGesUserClaimsService _gesUserClaimsService;
        private IGesRolesService _gesRolesService;
        private IOldUserService _oldUserService;
        private IG_IndividualsService _gIndividualService;
        private readonly IOrganizationService _organizationService;
        private GesUserManager _userManager;
        public GesUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<GesUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        #endregion

        #region Constructor
        public AccountMgmtController(IGesLogger logger, IGesUserService gesUserService, IGesUserRolesService gesUserRolesService,
            IGesUserClaimsService gesUserClaimsService, IGesRolesService gesRolesService,
            IOldUserService oldUserService, IG_IndividualsService gIndividualService, IOrganizationService organizationService) : base(logger)
        {
            _gesUserService = gesUserService;
            _gesUserRolesService = gesUserRolesService;
            _gesUserClaimsService = gesUserClaimsService;
            _gesRolesService = gesRolesService;
            _oldUserService = oldUserService;
            _gIndividualService = gIndividualService;
            _organizationService = organizationService;
        }
        #endregion

        #region ActionResult
        [CustomAuthorize(FormKey = "Account", Action = ActionEnum.Read)]
        public ActionResult List()
        {
            ViewBag.Title = "Account List";
            var roles = _gesRolesService.GetRolesInString();
            ViewBag.Roles = roles;

            return View();
        }

        [CustomAuthorize(FormKey = "Account", Action = ActionEnum.Read)]
        public ActionResult Details(string id)
        {
            var viewmodel = new GesUserViewModel();
            var listRoleNames = new List<string>();
            var listClaimTypes = new List<string>();


            if (!id.Equals("Add"))
            {

                var allGesUsers = _gesUserService.GetAllGesUsers();
                var listOldUsers = _oldUserService.GetListOldUsers();

                var model = (from q in allGesUsers
                    join o in listOldUsers on q.OldUserId equals o.G_Users_Id
                    where q.Id == id
                    select new GesAccountViewModel
                    {
                        Id = q.Id,
                        UserName = q.UserName,
                        Email = q.Email,
                        OldUserId = q.OldUserId,
                        RoleList = q.RoleList,
                        FirstName = o.G_Individuals1?.FirstName ?? "",
                        LastName = o.G_Individuals1?.LastName ?? "",
                        OrgName = o.G_Individuals1?.G_Organizations?.Name ?? "",
                        OrgId = o.G_Individuals1?.G_Organizations_Id,
                        Title = o.G_Individuals1?.JobTitle,
                        Phone = o.G_Individuals1?.Phone,
                        MobilePhone = o.G_Individuals1.MobilePhone,
                        WorkPhone = o.G_Individuals1.WorkPhone,
                        Fax = o.G_Individuals1.Fax,
                        Comments = o.G_Individuals1?.Comment,
                        Password = o.Password,
                        OldUserName = o.Username,
                        IsLocked = q.IsLocked,
                        LastLogIn = o.LastLogIn != null &&
                                    (q.LastLogIn != null &&
                                     DateTime.Compare((DateTime) q.LastLogIn, (DateTime) o.LastLogIn) >= 0)
                            ? q.LastLogIn
                            : o.LastLogIn
                    }).FirstOrDefault();



                if (model != null && model.RoleList.Any())
                {
                    var roles = _gesRolesService.GetGesRoles();
                    var listRoleIds = model.RoleList.ToList();
                    listRoleNames = roles.Where(d => listRoleIds.Contains(d.Id)).Select(d => d.Name).ToList();
                }



                var claims = UserManager.GetClaimsAsync(model.Id);

                if (claims != null && claims.Result.Any())
                {
                    listClaimTypes = claims.Result.Select(d => d.Type).ToList();
                }

                ViewBag.Title = "Account: " + model.UserName;
                var editViewmodel = new GesUserViewModel()
                {
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Roles = GetGesRoleSelectListItems(),
                    SelectedRoles = listRoleNames.ToArray(), //LoadUserRolesInString(model)
                    OrgName = model.OrgName,
                    OrgId = model.OrgId,
                    OldUserName = model.OldUserName,
                    Password = model.Password,
                    IsLocked = model.IsLocked,
                    Title = model.Title,
                    Phone = model.Phone,
                    MobilePhone = model.MobilePhone,
                    WorkPhone = model.WorkPhone,
                    Fax = model.Fax,
                    Comment = model.Comments,
                    Claims = GetGesClaimSelectListItems(),
                    LastLogIn = model.LastLogIn,
                    SelectedClaims = listClaimTypes.ToArray(),
                    Organizations = GetOrganizationListItems()

                };
                viewmodel = editViewmodel;
            }
            else
            {
                ViewBag.Title = "Add new account";
                
                viewmodel.Roles = GetGesRoleSelectListItems();
                viewmodel.Claims = GetGesClaimSelectListItems();
                viewmodel.Organizations = GetOrganizationListItems().ToArray();

            }

            ViewBag.Id = id;

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult CreateForm(long PortfolioId = 0, bool showInCSC = true)
        {
            if (PortfolioId <= 0)
                return PartialView("_CreateUser", new GesUserViewModel()
                {

                });

            return PartialView("_CreateUser", new GesUserViewModel()
            {

            });
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAccount(string accountId)
        {
            await Task.FromResult<object>(null);
            
            var gesUser = _gesUserService.GetById(accountId);

            if (gesUser == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Can not delete this account). Please try again.",
                    error = "Invalid model"
                });
            }

            G_Users gUser = null;
            G_Individuals gIndividual = null;

            try
            {
                if (gesUser.OldUserId != null)
                {
                    gUser = _oldUserService.GetUserById((long) gesUser.OldUserId);

                    if (gUser != null)
                    {
                        gIndividual = _gIndividualService.GetById(gUser.G_Individuals_Id);
                    }
                
                }

                var existingClaims = _gesUserClaimsService.GetByUserId(accountId);
                // remove claims
                foreach (var claim in existingClaims)
                {
                    _gesUserClaimsService.Delete(claim, true);
                }

                // get user's existing roles
                var existingRoles = _gesUserRolesService.GetByUserId(accountId);
                // remove roles
                foreach (var eRole in existingRoles)
                {
                    _gesUserRolesService.Delete(eRole, true);
                }
            
                _gesUserService.Delete(gesUser, true);

                if (gUser != null)
                {
                    _oldUserService.Delete(gUser, true);
                }

                if (gIndividual != null)
                {
                    _gIndividualService.Delete(gIndividual, true);
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Can not delete this account). Please try again.",
                    error = e.Message
                });
            }
            return Json(new
            {
                success = true,
                redirectUrl = Url.Action("List", "AccountMgmt")
            });
        }
        
        [HttpPost]
        public async Task<ActionResult> CreateAccount(GesUserViewModel model)
        {
            if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Invalid model). Please try again.",
                    error = "Invalid model"
                });

            var updating = false;

            var newAccountId = "";
            try
            {
                // if existed > update
                if (!string.IsNullOrEmpty(model.Id) && !model.Id.Equals("Add"))
                {
                    var existed = _gesUserService.GetById(model.Id);
                    if (existed != null)
                    {
                        updating = true;

                        // update roles
                        var updateRolesResult = UpdateUserRoles(model.SelectedRoles, existed);
                        if (!updateRolesResult)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Failed updating roles."
                            });
                        }
                    
                        // update claims
                        var updateClaimResult = UpdateUserClaims(model.SelectedClaims, existed);
                        if (!updateClaimResult)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Failed updating claims."
                            });
                        }
                        
                        // update properties
                        existed.Email = model.Email.Trim().ToLower();
                        existed.UserName = model.UserName.Trim();

                        if (model.IsLocked && !existed.LockoutEnabled)
                        {
                            existed.LockoutEnabled = true;
                            existed.LockoutEndDateUtc = DateTime.UtcNow.AddYears(100);
                        }
                        else if (!model.IsLocked)
                        {
                            existed.LockoutEnabled = false;
                            existed.LockoutEndDateUtc = null;
                        }

                        try
                        {
                            _gesUserService.Update(existed, true);

                            //update new email to table G_Individuals
                            var oldUser = _oldUserService.GetUserById(existed.OldUserId ?? -1);
                            if (oldUser != null)
                            {
                                var gIndividual = _gIndividualService.GetById(oldUser.G_Individuals_Id);
                                if (gIndividual != null)
                                {
                                    gIndividual.Email = existed.Email;
                                    gIndividual.G_Organizations_Id = model.OrgId;
                                    gIndividual.FirstName = model.FirstName != null?model.FirstName.Trim():"";
                                    gIndividual.LastName = model.LastName != null?model.LastName.Trim():"";
                                    gIndividual.JobTitle = model.Title;
                                    gIndividual.Phone = model.Phone;
                                    gIndividual.Comment = model.Comment;
                                    _gIndividualService.Update(gIndividual, true);
                                }


                                //change password on GesUser
                                if (oldUser.Password != model.Password)
                                {
                                    await UpdatePassword(existed.Id, model.Password);
                                }

                                if (oldUser.Username != model.UserName || oldUser.Password != model.Password)
                                {
                                    oldUser.Username = model.UserName;
                                    oldUser.Password = model.Password;
                                    _oldUserService.Update(oldUser, true);
                                }

                                ClearUserCache();
                                
                                UpdateGesUser(model, oldUser.G_Users_Id );
                            }
                        }
                        catch (Exception e)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Something went wrong while updating (Exception occurred). Please try again.",
                                error = e.Message
                            });
                        }
                    }
                    else // found nothing to update
                    {
                        return Json(new
                        {
                            success = false,
                            message = "No record matched in Database."
                        });
                    }
                }

                if (!updating)
                {
                    long gIndividualsId;
                    //Save Individual
                    try
                    {
                        gIndividualsId = UpdateIndividual(model);
                    }
                    catch (Exception e)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "Failed updating Individual.",
                            error = e.Message
                        });
                    }

                    if (gIndividualsId != 0)
                    {
                        //Save G_user (old)
                        long oldUserId;
                        try
                        {
                            oldUserId =  UpdateGUser(model, gIndividualsId );
                        }
                        catch (Exception e)
                        {
                            return Json(new
                            {
                                success = false,
                                message = "Failed updating G_user.",
                                error = e.Message
                            });
                        }

                        //Save Ges_user
                        if (oldUserId != 0)
                        {
                            try
                            {
                               var  newGesUser = UpdateGesUser(model, oldUserId );
                                
                                if (newGesUser != null)
                                {
                                    // save roles
                                    var updateRolesResult = UpdateUserRoles(model.SelectedRoles, newGesUser);
                                    if (!updateRolesResult)
                                    {
                                        return Json(new
                                        {
                                            success = false,
                                            message = "Failed updating roles."
                                        });
                                    }
                            
                                    // save claims
                                    var updateClaimResult = UpdateUserClaims(model.SelectedClaims, newGesUser);
                                    if (!updateClaimResult)
                                    {
                                        return Json(new
                                        {
                                            success = false,
                                            message = "Failed updating claims."
                                        });
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                return Json(new
                                {
                                    success = false,
                                    message = "Failed updating Ges User.",
                                    error = e.Message
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    success = false,
                    message = "Something went wrong (Exception occurred). Please try again.",
                    error = e.Message
                });
            }

            return Json(new
            {
                success = true,
                editing = updating,
                redirectUrl = updating ? "" : (Url.Action("Details", "AccountMgmt") + string.Format("?id={0}", newAccountId))
            });
        }

        private long UpdateIndividual(GesUserViewModel model)
        {

            var gIndividual = new G_Individuals
            {
                Email = model.Email,
                G_Organizations_Id = model.OrgId,
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                JobTitle = model.Title,
                Phone = model.Phone,
                Comment = model.Comment,
                G_TimeZones_Id = 27,
                BillingManualAdjustment = 0,
            };

            _gIndividualService.Add(gIndividual, true);

            return gIndividual.G_Individuals_Id;
        }

        private long UpdateGUser(GesUserViewModel model, long gIndividualsId)
        {

            var currentUserId = _gesUserService.GetById(User.Identity.GetUserId()).OldUserId;
            
            var gUser = new G_Users
            {
                G_Individuals_Id = gIndividualsId,
                Username = model.UserName,
                Password = model.Password,
                Created = DateTime.UtcNow,
                ModifiedByG_Users_Id =  currentUserId,
                LogInCount = 0
            };
            
            _oldUserService.Add(gUser, true);
            ClearUserCache();

            return gUser.G_Users_Id;
        }
        
        private GesUser UpdateGesUser(GesUserViewModel model, long oldUserId)
        {
           
            var passwordHash = new PasswordHasher();
            var password = passwordHash.HashPassword(model.Password);
            GesUser gesUser;

            if (model.Id == "Add")
            {
                gesUser = new GesUser
                {
                    Id = Guid.NewGuid().ToString(),
                    OldUserId = oldUserId,
                    Email = model.Email,
                    EmailConfirmed = false,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = model.IsLocked,
                    AccessFailedCount = 0,
                    UserName = model.UserName,
                    PhoneNumber = model.Phone,
                    PasswordHash = password,
                    SecurityStamp = ""
                };
                _gesUserService.Add(gesUser, true);
            }
            else
            {
                gesUser = _gesUserService.GetByOldUserId(oldUserId);
                
                gesUser.Email = model.Email;
                gesUser.PhoneNumber = model.Phone;
                gesUser.PasswordHash = password;
                gesUser.SecurityStamp = "";

                if (gesUser.LockoutEnabled != model.IsLocked)
                {
                    gesUser.LockoutEnabled = model.IsLocked;
                    if (!model.IsLocked)
                    {
                        gesUser.LockoutEndDateUtc = DateTime.UtcNow;
                    }
                }
                
                _gesUserService.Update(gesUser, true);

            }

            return gesUser;
        }        
   
        public async Task<bool> UpdatePassword(string userId, string newPassword)
        {
            var usermager = HttpContext.GetOwinContext().GetUserManager<GesUserManager>();
            usermager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            var token = await usermager.GeneratePasswordResetTokenAsync(userId);
            var result = await usermager.ResetPasswordAsync(userId, token, newPassword.Trim());

            return result.Succeeded;
        }
        #endregion

        #region JsonResult

        [HttpPost]
        public JsonResult GetDataForGesUsersJqGrid(JqGridViewModel jqGridParams)
        {

            var allGesUsers = _gesUserService.GetAllGesUsers();
            var listOldUsers = _oldUserService.GetListOldUsers();

            var query = (from q in allGesUsers
                         join o in listOldUsers on q.OldUserId equals o.G_Users_Id
                         select new GesAccountViewModel
                         {
                             Id = q.Id,
                             UserName = q.UserName,
                             OldUserName = o.Username,
                             Email = q.Email??"",
                             OldUserId = q.OldUserId,
                             RoleList = q.RoleList,
                             FirstName = o.G_Individuals1?.FirstName ?? "",
                             LastName = o.G_Individuals1?.LastName ?? "",
                             OrgName = o.G_Individuals1?.G_Organizations?.Name ?? "",
                             IsLocked = q.IsLocked,
                             Status = q.IsLocked?"Locked":"Active",
                             ClaimsString = string.Join(", ", q.Claims)
                         }).ToList().AsEnumerable();

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
                    case "orgname":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.OrgName).ThenBy(d => d.UserName)
                            : query.OrderByDescending(x => x.OrgName).ThenBy(d => d.UserName);
                        break;
                    case "oldusername":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.OldUserName).ThenBy(d => d.UserName)
                            : query.OrderByDescending(x => x.OldUserName).ThenBy(d => d.UserName);
                        break;
                    case "status":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.IsLocked).ThenBy(d => d.UserName)
                            : query.OrderByDescending(x => x.IsLocked).ThenBy(d => d.UserName);
                        break;
                    case "claimsstring":
                        query = sortDir == "asc"
                            ? query.OrderBy(x => x.ClaimsString).ThenBy(d => d.UserName)
                            : query.OrderByDescending(x => x.ClaimsString).ThenBy(d => d.UserName);
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

            var filteredRecords = query.Count();
            var items = query.Skip((jqGridParams.page - 1) * jqGridParams.rows).Take(jqGridParams.rows).ToList();

            var numPages = (int)Math.Ceiling((float)filteredRecords / (float)jqGridParams.rows);

            return Json(new
            {
                total = numPages,
                page = jqGridParams.page,
                records = filteredRecords,
                rows = items
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckEmailExist(string email, string Id)
        {
            var allGesUsers = _gesUserService.GetAllGesUsers();

            email = email.Trim().ToLower();
            return Json(!allGesUsers.Any(i => i.Id != Id && i.Email != null && i.Email.Trim().ToLower() == email),
                JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult CheckUserNameExist(string username, string Id)
        {
            var allGesUsers = _gesUserService.GetAllGesUsers();

            username = username.Trim().ToLower();
            return Json(!allGesUsers.Any(i => i.Id != Id && i.UserName.Trim().ToLower() == username),
                JsonRequestBehavior.AllowGet);
        }

        public JsonResult SyncUsers()
        {
            int existedEmail, duplicateEmail, addedSucess, passwordSync;

            if (TrySyncUser(out existedEmail, out duplicateEmail, out addedSucess, out passwordSync))
            {
                return Json(
                    new
                    {
                        AddedItems = addedSucess,
                        ExistedEmail = existedEmail,
                        DuplicatedEmail = duplicateEmail,
                        PasswordSync = passwordSync
                    }, JsonRequestBehavior.AllowGet
                );

            }
            return Json(new
            {
                success = false,
                message = "Failed sync users."
            });
        }

        #endregion

        #region Private methods

        private static void ClearUserCache()
        {
            QueryCacheManager.ExpireTag(Configurations.AllGesUsersCache);
            QueryCacheManager.ExpireTag(Configurations.AllOldUsersCache);
        }

        private bool TrySyncUser(out int existedEmail, out int duplicateEmail, out int addedSucess, out int passwordSync)
        {
            existedEmail = 0;
            duplicateEmail = 0;
            addedSucess = 0;
            passwordSync = 0;

            try
            {
                ClearUserCache();

                var allGesUsers = _gesUserService.GetAllGesUsers();
                var listOldUsers = _oldUserService.GetListOldUsers();
                
                var listOldUserIdFromGesTable = allGesUsers.Where(d => d.OldUserId != null).Select(d => d.OldUserId.Value).ToList();
                var listOldUserNotSync = listOldUsers.Where(d => !listOldUserIdFromGesTable.Contains(d.G_Users_Id)).OrderBy(d => d.Username).ThenByDescending(d => d.LastLogIn).ThenByDescending(d => d.G_Users_Id).ToList();
                var listNewUser = new List<GesUser>();
                var userName = "";
                var duplicateAccount = 0;
                var listUpdatePassword = allGesUsers.Where(p => string.IsNullOrEmpty(p.Password)).ToList();
                
                foreach (var d in listOldUserNotSync)
                {
                    var passwordHash = new PasswordHasher();
                    var password = passwordHash.HashPassword(d.Password);

                    //check duplicate UserName:
                    // if the userName is duplicated => add a number to userName
                    if (userName != d.Username.Trim().ToLower())
                    {
                        userName = d.Username.Trim().ToLower();
                        duplicateAccount = 0;
                    }
                    else
                    {
                        duplicateAccount += 1;
                    }

                    var newUserName = (duplicateAccount > 0) ? d.Username.Trim() + "_" + duplicateAccount : d.Username.Trim();

                    var email = string.Empty;
                    var phoneNumber = string.Empty;
                    if (d.G_Individuals1 != null)
                    {
                        email = d.G_Individuals1.Email?.Trim().ToLower();
                        phoneNumber = d.G_Individuals1.Phone;
                    }

                    if (email == null || UtilHelper.IsEmailAddress(email))
                    {
                        //check exist email
                        if (email != null && allGesUsers.FirstOrDefault(dx => dx.Email != null && dx.Email.Trim().Equals(email, StringComparison.OrdinalIgnoreCase)) != null)
                        {
                            existedEmail += 1;
                            continue;
                        }

                        //create newUsername
                        while (allGesUsers.FirstOrDefault(dx => dx.UserName.Trim().Equals(newUserName, StringComparison.OrdinalIgnoreCase)) != null)
                        {
                            duplicateAccount += 1;

                            newUserName = d.Username.Trim() + "_" + duplicateAccount;
                        }

                        var newUserId = Guid.NewGuid().ToString();
                        var lockoutEnable = d.Password.ToLower().Contains("locked");
                        DateTime? dateEndLocked = null;
                        if (lockoutEnable) dateEndLocked = DateTime.UtcNow.AddYears(100).Date;

                        listNewUser.Add(new GesUser { Id = newUserId, UserName = newUserName, LockoutEnabled = lockoutEnable,
                            LockoutEndDateUtc = dateEndLocked, PasswordHash = password, SecurityStamp = "", Email = email,
                            PhoneNumber = phoneNumber, OldUserId = d.G_Users_Id, LastLogIn = d.LastLogIn});
                    }
                }

                //remove account with duplicate Email (keep account with lastest login)
                var listDuplicateEmail =
                    listNewUser
                        .GroupBy(c => c.Email)
                        .Where(grp => grp.Key != null && grp.Count() > 1)
                        .Select(grp => grp.Key).ToList();

                if (listDuplicateEmail.Any())
                {
                    foreach (var emailItem in listDuplicateEmail)
                    {
                        var list = listNewUser.Where(d => d.Email == emailItem).OrderByDescending(d => d.LastLogIn).ThenByDescending(d => d.OldUserId).ToList();
                        var keepEmail = list.FirstOrDefault();

                        foreach (var itemDelete in list)
                        {
                            if (itemDelete.Id != keepEmail.Id)
                            {
                                listNewUser.Remove(itemDelete);
                                duplicateEmail += 1;
                            }
                        }
                    }
                }

                foreach (var gesUser in listNewUser)
                {
                    _gesUserService.Add(gesUser);
                }

                //Sync password
                foreach (var item in listUpdatePassword)
                {
                    if (item.OldUserId == null) continue;
                    
                    var oldUser = _oldUserService.GetUserById((long) item.OldUserId);
                    var passwordHash = new PasswordHasher();
                    var password = passwordHash.HashPassword(oldUser.Password);

                    var updateUserPassword = _gesUserService.GetById(item.Id);
                    updateUserPassword.PasswordHash = password;
                    _gesUserService.Update(updateUserPassword);
                    passwordSync += 1;

                }

                addedSucess = _gesUserService.Save();

                return true;
            }
            catch (Exception e)
            {
                Logger?.Error(e, "Exception when sync user.");
                return false;
            }
        }

        private bool UpdateUserRoles(List<GesUserRole> newUserRoles, string userId)
        {
            bool hasChangeData = false;
            var listId = newUserRoles.ToList().Select(d => d.RoleId);

            var oldUserRoles = _gesUserRolesService.GetByUserId(userId).ToList();

            //Delete Item
            var listDelete = oldUserRoles.Where(d => !listId.Contains(d.UserId));

            foreach (var deleteItem in listDelete)
            {
                _gesUserRolesService.Delete(deleteItem);
                hasChangeData = true;
            }

            //Add newItem
            foreach (var newItem in newUserRoles)
            {
                if (oldUserRoles.All(d => d.RoleId != newItem.RoleId))
                {
                    _gesUserRolesService.Add(newItem);
                    hasChangeData = true;
                }
            }

            return _gesUserRolesService.Save() > 0 || hasChangeData == false;
        }

        private IEnumerable<SelectListItem> GetGesRoleSelectListItems()
        {
            var roles = _gesRolesService.GetGesRoles()
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Name,
                                    Text = x.Name
                                });

            var list = new SelectList(roles, "Value", "Text");
            return list;
        }

        private IEnumerable<SelectListItem> GetGesClaimSelectListItems()
        {
            var claim = Enum.GetValues(typeof(ClaimEnum))
                        .Cast<ClaimEnum>()
                        .Select(v => v.GetEnumDescription())
                        .ToList();
            
            var roles = claim.Select(x =>
                                new SelectListItem
                                {
                                    Value = x.ToString(),
                                    Text = x.ToString()
                                });

            var list = new SelectList(roles, "Value", "Text");
            return list;
        }

        private IEnumerable<SelectListItem> GetOrganizationListItems()
        {
            var organization = _organizationService.GetAllClients().Select(x =>
                new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name
                });

            var list = new SelectList(organization, "Value", "Text");
            return list;
        }

        
        private string[] LoadUserRolesInString(GesUser user)
        {
            return _gesRolesService.RolesToStringArray(user.Roles);
        }


        private bool UpdateUserRoles(IEnumerable<string> roles, GesUser user)
        {
            if (user == null) return false;

            try
            {
                // case: roles null
                roles = roles ?? new List<string>();

                // get user's existing roles
                var roleManager = new RoleManager<GesRole>(new GesRoleStore(new GesRefreshDbContext()));
                var existingRoles = user.Roles.Select(existing => roleManager.FindById(existing.RoleId))
                    .Select(roleItem => roleItem.Name).ToList();

                // remove roles
                foreach (var eRole in existingRoles)
                {
                    if (!roles.Contains(eRole))
                        UserManager.RemoveFromRole(user.Id, eRole);
                }

                // add roles
                foreach (var role in roles)
                {
                    UserManager.AddToRole(user.Id, role);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private bool UpdateUserClaims(IEnumerable<string> claims, GesUser user)
        {
            if (user == null) return false;

            try
            {
                var result = true;
                // case: claims null
                claims = claims ?? new List<string>();

                var existingClaims = user.Claims.Select(d=>new Claim(d.ClaimType, d.ClaimValue));
                var removed = true;
                // remove roles
                foreach (var eRole in existingClaims)
                {
                    var resultRemove = UserManager.RemoveClaimAsync(user.Id, eRole);

                    if (!resultRemove.Result.Succeeded)
                    {
                        removed = false;
                        break;
                    }
                }

                // add claims
                if (removed)
                {
                    foreach (var claim in claims)
                    {
                        var resultAdd = UserManager.AddClaimAsync(user.Id, new Claim(claim, "True"));

                        if (!resultAdd.Result.Succeeded)
                        {
                            result = false;
                            break;
                        }
                    }
                }

                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}