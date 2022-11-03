using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GES.Common.Enumeration;
using GES.Common.Extensions;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using GES.Inside.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GES.Inside.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private GesUserManager _userManager;
        private readonly IGesUserService _gesUserService;
        
        public AuthenticationController(IGesUserService gesUserService)
        {
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
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            var currentUser = _gesUserService.GetByUserName(model.Login);

            if (currentUser != null)
            {
                // only allow @ges-invest.com users + joakim.westin + sustainalytics.com
                //if (!(currentUser.Email.Contains(Configurations.OldEmailDomain) || currentUser.Email.Contains(Configurations.EmailDomain) || currentUser.Email.Contains(Configurations.SustainDomain) || currentUser.Email.Contains("joakim@jwab.net")))
                if(currentUser.Claims.All(d => d.ClaimType != ClaimEnum.AccessInside.GetEnumDescription()))
                {
                    return View("~/Views/Error/UnauthorizedSite.cshtml");
                }
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

            return RedirectToAction("Login", "Authentication");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> ForgotPassword(LoginViewModel model)
        {
            await Task.FromResult<LoginViewModel>(model);
            return View(model);
        }
    }
}