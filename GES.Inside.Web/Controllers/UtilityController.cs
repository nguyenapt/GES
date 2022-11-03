using System.Web.Mvc;
using GES.Common.Enumeration;

namespace GES.Inside.Web.Controllers
{
    public class UtilityController : Controller
    {
        [CustomAuthorize(FormKey = "FormatISIN", Action = ActionEnum.Read)]
        public ActionResult FormatIsins()
        {
            return View();
        }

        [CustomAuthorize(FormKey = "UtilityTool", Action = ActionEnum.Read)]
        public ActionResult Tools()
        {
            return View();
        }
    }
}