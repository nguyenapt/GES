using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace GES.Inside.Web
{
    public class InsideAuthorization : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            if (context.Principal.IsInRole("Admin") || context.Principal.IsInRole("SuperAdmin"))
            {
                return Ok();
            }

            return CheckPermissionAsync(context);
        }

        private Task<bool> CheckPermissionAsync(ResourceAuthorizationContext context)
        {
            var action = context.Action.First().Value;
            var resource = context.Resource.First().Value;

            if (context.Principal.HasClaim(resource, action))
            {
                return Ok();
            }
            return Nok();
        }
    }
}