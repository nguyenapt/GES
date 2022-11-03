using System;
using GES.Inside.Data.Models.Auth;
using GES.Inside.Data.DataContexts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace GES.Inside.Data.Services.Auth
{
    public class GesUserManager : UserManager<GesUser, string>
    {
        public GesUserManager(IUserStore<GesUser, string> store)
            : base(store)
        {
            
        }

        public static GesUserManager Create(IdentityFactoryOptions<GesUserManager> options, IOwinContext context)
        {
            var store = new GesUserStore(context.Get<GesRefreshDbContext>());

            var manager = new GesUserManager(store);
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<GesUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //Configure reset password token render
            var provider = new MachineKeyProtectionProvider();
            manager.UserTokenProvider = new DataProtectorTokenProvider<GesUser>(provider.Create("ResetPasswordPurpose"));

            return manager;
        }

    }
}
