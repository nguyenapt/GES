using System.Web.Mvc;

namespace GES.Clients.Web.Controllers
{
    public class LegalController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("TermsOfUse");
        }

        public ActionResult TermsOfUse()
        {
            return View();
        }

        public ActionResult CookiePolicy()
        {
            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}