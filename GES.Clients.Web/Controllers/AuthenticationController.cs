using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GES.Clients.Web.Models;
using GES.Common.Enumeration;
using GES.Common.Services.Interface;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GES.Clients.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private GesUserManager _userManager;
        private readonly IMailService _mailService;
        private IOldUserService _oldUserService;
        private IPermissionService _permissionService;
        private II_CompaniesService _companiesService;
        private readonly IG_IndividualsService _gIndividualsService;
        private readonly II_EngagementTypesService _engagementTypesService;
        private readonly IGesUserService _gesUserService;

        public AuthenticationController(IMailService mailService, IOldUserService oldUserService, IPermissionService permissionService, IG_IndividualsService gIndividualsService, II_EngagementTypesService engagementTypesService, II_CompaniesService companiesService, IGesUserService gesUserService)
        {
            _mailService = mailService;
            _oldUserService = oldUserService;
            _permissionService = permissionService;
            _gIndividualsService = gIndividualsService;
            _engagementTypesService = engagementTypesService;
            _companiesService = companiesService;
            _gesUserService = gesUserService;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var result = await SignInManager.PasswordSignInAsync(model.Login, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    var userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, model.Login));

                    var claimsIdentity = new ClaimsIdentity(userClaims, DefaultAuthenticationTypes.ApplicationCookie);
                    var ctx = Request.GetOwinContext();
                    
                    var authenticationManager = ctx.Authentication;
                    authenticationManager.SignIn(claimsIdentity);

                    var usermager = HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();
                    
                    var currentUser = _gesUserService.GetByUserName(model.Login);
                    
                    long oldId = currentUser?.OldUserId ?? -1;
                    var individual = _gIndividualsService.GetIndividualByUserId(oldId);

                    if (!_gIndividualsService.CheckServiceForIndividual(individual?.G_Individuals_Id ?? -1))
                    {
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return Redirect("https://clients.ges-invest.com");
                    }

                    Session["IndividualId"] =  individual?.G_Individuals_Id ?? -1;
                    Session["OrgId"] = individual?.G_Organizations_Id ?? -1;
                    Session["FirstName"] = individual?.FirstName ?? String.Empty;
                    Session["LastName"] = individual?.LastName ?? String.Empty;

                    if (currentUser != null)
                    {
                        currentUser.LastLogIn = DateTime.UtcNow;
                        
                        _gesUserService.Update(currentUser);
                        _gesUserService.Save();
                    }

                    if (returnUrl != null)
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                case SignInStatus.LockedOut:
                    return RedirectToAction("Login", "Authentication");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        private bool ValidateUser(string login, string password)
        {
            return login == password;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            Session["IndividualId"] = null;
            Session["OrgId"] = null;

            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                            {
                                success = false,
                                message = "",
                                error = "Something wrong. Invalid model! Kindly check again."
                            });
            }

            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Json(new
                            {
                                success = false,
                                message = "",
                                error = "Kindly check again. We could not find any account associated with that email."
                            });
            }

            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var callbackUrl = Url.Action("ResetPassword", "Authentication", new {userId = user.Id, code = token}, Request.Url.Scheme);

            await _mailService.SendResetPasswordMail(callbackUrl, user.UserName, user.Email);

            return Json(new
                        {
                            success = true,
                        });
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirm()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string userId, string code)
        {
            if (code == null || userId == null)
            {
                return View("Error");
            }
            return View(new ResetPasswordViewModel
                        {
                            UserId = userId,
                            Code = code,
                        });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<JsonResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "Request is invalid!"
                });

            }
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "",
                    error = "User not found!"
                });
            }
            var decode = WebUtility.HtmlDecode(model.Code);
            var result = await UserManager.ResetPasswordAsync(user.Id, decode, model.Password);
            if (result.Succeeded)
            {
                var oldUser = _oldUserService.GetUserById(user.OldUserId ?? -1);
                if (oldUser != null)
                {
                    oldUser.Password = model.ConfirmPassword;

                    _oldUserService.Update(oldUser, true);
                }

                return Json(new
                {
                    success = true,
                    message = "Password has been reset successfully.",
                    error = ""
                });
            }
            return Json(new
            {
                success = false,
                message = "",
                error = string.Join("; ", result.Errors)
            });
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordConfirm()
        {
            return View();
        }


        public ActionResult RenderSidebar(string queryString, string action, string controller)
        {
            long orgId = 0, individualId = 0;
            GetIndividualInfo(ref individualId, ref orgId);

            var organizationServices = _permissionService.GetOrganizationsG_ServicesByOrgId(orgId);
            var listServiceIds = organizationServices.Select(d => d.G_Services_Id).ToList();

            var model = new SidebarModel
            {
                Action = action,
                Controller = controller,
                QueryString = queryString,

                AlertService = listServiceIds.Contains((int)GesService.GesAlertServices),
                GlobalEthical = listServiceIds.Contains((int)GesService.GesGlobalEthicalStandard),
                GesControversial = listServiceIds.Contains((int)GesService.GesControversial),
                ClientType = _companiesService.GetClientType(orgId),
                //BusinessConduct = listServiceIds.Contains((int)GesService.BusinessConduct),
                //Burnma = listServiceIds.Contains((int)GesService.Burnma),
                //CarbonRisk = listServiceIds.Contains((int)GesService.CarbonRisk),
                //EmerginMarkets = listServiceIds.Contains((int)GesService.EmerginMarkets),
                //PalmOil = listServiceIds.Contains((int)GesService.PalmOil),
                //Taxation = listServiceIds.Contains((int)GesService.Taxation),
                //Water = listServiceIds.Contains((int)GesService.Water),
                //Governance = listServiceIds.Contains((int)GesService.Governance)
                EngagementTypeCategoryViews = GetEngagementTypeCategoryViews(orgId)
            };
            return PartialView("_Sidebar", model);
        }

        private void GetIndividualInfo(ref long individualId, ref long orgId)
        {
            if (Session["OrgId"] == null || Session["IndividualId"] == null)
            {
                var usermager = HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(User.Identity.GetUserId());
                long oldId = currentUser?.OldUserId ?? -1;

                var individual = _gIndividualsService.GetIndividualByUserId(oldId);

                Session["IndividualId"] = individualId = individual?.G_Individuals_Id ?? -1;
                Session["OrgId"] = orgId = individual?.G_Organizations_Id ?? -1;
            }
            else
            {
                individualId = (long)Session["IndividualId"];
                orgId = (long)Session["OrgId"];
            }
        }

        private IEnumerable<EngagementTypeCategoryView> GetEngagementTypeCategoryViews(long orgId)
        {
            var engagementTypeCategories = _engagementTypesService.AllEngagementTypeCategories()
                .Select(x => new {x.I_EngagementTypeCategories_Id, x.Name});

            return engagementTypeCategories.Select(category => new EngagementTypeCategoryView()
                {
                    EngagementTypeCategoriesId = category.I_EngagementTypeCategories_Id,
                    Name = category.Name,
                    EngagementTypeViewModels = _engagementTypesService
                        .GetEngagementTypeModelByCategoryId(category.I_EngagementTypeCategories_Id, orgId).OrderBy(e => e.Name)
                })
                .ToList();

        }

    }
}