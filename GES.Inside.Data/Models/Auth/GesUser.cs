using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace GES.Inside.Data.Models.Auth
{
    public class GesUser : IdentityUser<string, GesUserLogin, GesUserRole, GesUserClaim>
    {
        public long? OldUserId { get; set;}
        public DateTime? LastLogIn { get; set;}

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<GesUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}