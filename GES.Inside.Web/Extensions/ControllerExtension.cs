using System.Web;
using System.Web.Mvc;
using GES.Inside.Data.Services.Auth;
using GES.Inside.Data.Services.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GES.Inside.Web.Extensions
{
    public static class ControllerExtension
    {
        public static void GetIndividualInfo(this Controller controller, IG_IndividualsService _gIndividualsService, ref long individualId, ref long orgId)
        {
            if (controller.Session["OrgId"] == null || controller.Session["IndividualId"] == null)
            {
                individualId = controller.GetIndividualIdId(_gIndividualsService);
                orgId = controller.GetOrganizationId(_gIndividualsService);
            }
            else
            {
                individualId = (long)controller.Session["IndividualId"];
                orgId = (long)controller.Session["OrgId"];
            }
        }

        public static long GetOrganizationId(this Controller controller, IG_IndividualsService _gIndividualsService)
        {
            if (controller.Session["OrgId"] == null)
            {
                var usermager = controller.HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(controller.User.Identity.GetUserId());
                long oldId = currentUser?.OldUserId ?? -1;

                var individual = _gIndividualsService.GetIndividualByUserId(oldId);

                var orgId = individual?.G_Organizations_Id ?? -1;

                controller.Session["OrgId"] = orgId;

                return orgId;
            }
            else
            {
                return (long)controller.Session["OrgId"];
            }
        }

        public static long GetIndividualIdId(this Controller controller, IG_IndividualsService _gIndividualsService)
        {
            if (controller.Session["IndividualId"] == null)
            {
                var usermager = controller.HttpContext.GetOwinContext()
                    .GetUserManager<GesUserManager>();

                var currentUser = usermager.FindById(controller.User.Identity.GetUserId());
                long oldId = currentUser?.OldUserId ?? -1;

                var individual = _gIndividualsService.GetIndividualByUserId(oldId);

                var orgId = individual?.G_Individuals_Id ?? -1;

                controller.Session["IndividualId"] = orgId;

                return orgId;
            }
            else
            {
                return (long)controller.Session["IndividualId"];
            }
        }
    }
}