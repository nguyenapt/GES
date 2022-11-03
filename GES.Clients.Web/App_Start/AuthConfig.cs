using System;
using System.Security.Claims;
using System.Web.Helpers;
using GES.Clients.Web;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Services.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartup(typeof(AuthConfig))]
namespace GES.Clients.Web
{
    public class AuthConfig
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            

            app.CreatePerOwinContext(GesRefreshDbContext.Create);
            //app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<GesUserManager>(GesUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            app.CreatePerOwinContext<RoleManager<GesRole>>((options, context) =>
                new RoleManager<GesRole>(new GesRoleStore(context.Get<GesRefreshDbContext>())));

            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Authentication/Login"),
                CookieSecure = CookieSecureOption.SameAsRequest,
                CookieHttpOnly=true,
                SlidingExpiration = true,
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<GesUserManager, GesUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });

            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            var cookie = new CookieAuthenticationOptions
            {
                AuthenticationType = "Cookie",
                CookieSecure = CookieSecureOption.SameAsRequest,
                CookieHttpOnly = true,
                SlidingExpiration = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(20),
                LoginPath = new PathString("/Authentication/Login"),
            };

            app.UseCookieAuthentication(cookie);
            app.UseResourceAuthorization(new InsideAuthorization());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}