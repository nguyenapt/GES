using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace GES.Clients.Web
{
    public class InsideAuthorization : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var resource = context.Resource.First().Value;

            //if (resource == InsideResources.Portfolio)
            //{
            //    return CheckTrackAccessAsync(context);
            //}

            if (resource == InsideResources.Portfolio)
            {
                return CheckInsidePortfolioAccessAsync(context);
            }

            return Nok();
        }

        private Task<bool> CheckTrackAccessAsync(ResourceAuthorizationContext context)
        {
            return Eval(context.Principal.IsInRole("Admin"));
        }

        private Task<bool> CheckInsidePortfolioAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
            {
                return Nok();
            }

            var action = context.Action.First().Value;
            if (action == InsideResources.PortfolioActions.Edit)
            {
                return CheckInsidePortfolioEditAccessAsync(context);
            }

            return Ok();
        }

        private Task<bool> CheckInsidePortfolioEditAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.IsInRole("SuperAdmin") && !context.Principal.IsInRole("Admin"))
            {
                return Nok();
            }

            //if (context.Resource.Count() == 2)
            //{
            //    return CheckAlbumEditAccessByIdAsync(context);
            //}

            return Ok();
        }

        //private Task<bool> CheckAlbumEditAccessByIdAsync(ResourceAuthorizationContext context)
        //{
        //    var id = context.Resource.Skip(1).Take(1).Single().Value;
        //    if (id == "1")
        //    {
        //        return Eval("bob".Equals(context.Principal.Identity.Name, StringComparison.OrdinalIgnoreCase));
        //    }

        //    return Ok();
        //}
    }
}