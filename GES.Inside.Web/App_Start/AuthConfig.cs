using System;
using System.Security.Claims;
using System.Web.Helpers;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.DataContexts;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using GES.Inside.Data.Services;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

[assembly: OwinStartup(typeof(GES.Inside.Web.AuthConfig))]
namespace GES.Inside.Web
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
                ExpireTimeSpan = TimeSpan.FromMinutes(20),
                LoginPath = new PathString("/Authentication/Login"),
            };

            app.UseCookieAuthentication(cookie);
            app.UseResourceAuthorization(new InsideAuthorization());

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
        }
    }
}