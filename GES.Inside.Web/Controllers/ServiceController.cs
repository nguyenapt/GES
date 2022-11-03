using System.Linq;
using System.Web.Mvc;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;

namespace GES.Inside.Web.Controllers
{
    public class ServiceController : GesControllerBase
    {
        #region Declaration
        private readonly II_ServicesService _servicesService;
        #endregion

        #region Constructor
        public ServiceController(IGesLogger logger, II_ServicesService servicesService)
            : base(logger)
        {
            _servicesService = servicesService;
        }
        #endregion

        #region ActionResult

        #endregion

        #region JsonResult
        public JsonResult GetServices()
        {
            var servicesList = this.SafeExecute(() => _servicesService.GetAll(), $"Error when getting all services.");

            var servicesListSimplified = servicesList.Select(i => new ValueTextModel
            {
                value = i.G_Services_Id,
                text = i.Name
            }).OrderBy(d=>d.text).ToList();

            return Json(servicesListSimplified, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Private methods

        #endregion

    }
}