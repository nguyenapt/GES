using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using GES.Common.Models;
using GES.Inside.Data.Models;
using GES.Inside.Data.Services.Interfaces;
using GES.Common.Logging;
using GES.Inside.Data.Extensions;
using GES.Inside.Data.Helpers;

namespace GES.Clients.Web.Controllers
{
    public class CalendarController : GesControllerBase
    {
        #region Declaration
        private readonly ICalendarService _calendarService;
        #endregion

        #region Constructor
        public CalendarController(IGesLogger logger, ICalendarService calendarService)
            : base(logger)
        {
            _calendarService = calendarService;
        }
        #endregion
        

        #region ActionResult

        public ActionResult CalendarList()
        {

            ViewBag.Title = "Calendars";

            return View();
        }
        
        #endregion
        
        #region JsonResult
        [HttpPost]
        public JsonResult GetEventsForJqgrid(JqGridViewModel jqGridParams, long companyId, bool onlyUpcomingEvents = true)
        {
            var events = _calendarService.GetCalendarEventsByCompanyId(companyId);
            if (onlyUpcomingEvents)
            {
                events = events.Where(x => x.EventDate.Date >= DateTime.UtcNow.Date).ToList();
            }

            var sortCol = jqGridParams.sidx.ToLower();
            var sortDir = jqGridParams.sord.ToLower();
            if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
            {
                switch (sortCol)
                {
                    case "heading":
                        events = sortDir == "asc"
                            ? events.OrderBy(x => x.Heading).ToList()
                            : events.OrderByDescending(x => x.Heading).ToList();
                        break;
                    case "description":
                        events = sortDir == "asc"
                            ? events.OrderBy(x => x.Description).ToList()
                            : events.OrderByDescending(x => x.Description).ToList();
                        break;
                    case "eventdate":
                        events = sortDir == "asc"
                            ? events.OrderBy(x => x.EventDate).ToList()
                            : events.OrderByDescending(x => x.EventDate).ToList();
                        break;
                    case "isgesevent":
                        events = sortDir == "asc"
                            ? events.OrderBy(x => x.IsGesEvent).ToList()
                            : events.OrderByDescending(x => x.IsGesEvent).ToList();
                        break;                    
                    case "alldayevent":
                        events = sortDir == "asc"
                            ? events.OrderBy(x => x.AllDayEvent).ToList()
                            : events.OrderByDescending(x => x.AllDayEvent).ToList();
                        break;
                }
            }

            return Json(events.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataForCalendarJqGrid(JqGridViewModel jqGridParams, bool onlyUpcomingEvents = true)
        {

            try
            {
                var events = _calendarService.GetCalendarEvents();

                if (onlyUpcomingEvents)
                {
                    events = events.Where(x => x.EventDate.Date >= DateTime.UtcNow.Date).ToList();
                }

                var sortCol = jqGridParams.sidx.ToLower();
                var sortDir = jqGridParams.sord.ToLower();
                if (!(string.IsNullOrEmpty(sortCol) && string.IsNullOrEmpty(sortDir)))
                {
                    switch (sortCol)
                    {
                        case "heading":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.Heading).ToList()
                                : events.OrderByDescending(x => x.Heading).ToList();
                            break;
                        case "description":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.Description).ToList()
                                : events.OrderByDescending(x => x.Description).ToList();
                            break;
                        case "eventdate":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.EventDate).ToList()
                                : events.OrderByDescending(x => x.EventDate).ToList();
                            break;
                        case "eventenddate":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.EventEndDate).ToList()
                                : events.OrderByDescending(x => x.EventEndDate).ToList();
                            break;
                        case "companynameorengagementname":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.CompanyNameOrEngagementName).ToList()
                                : events.OrderByDescending(x => x.CompanyNameOrEngagementName).ToList();
                            break;
                        case "isgesevent":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.IsGesEvent).ToList()
                                : events.OrderByDescending(x => x.IsGesEvent).ToList();
                            break;
                        case "eventlocation":
                            events = sortDir == "asc"
                                ? events.OrderBy(x => x.EventLocation).ToList()
                                : events.OrderByDescending(x => x.EventLocation).ToList();
                            break;
                    }
                }

                if (jqGridParams._search)
                {
                    var finalRules = JqGridHelper.GetFilterRules<EventListViewModel>(jqGridParams);
                    events = string.IsNullOrEmpty(finalRules) ? events : events.Where(finalRules);
                }

                return Json(events.ToPagedList(Logger, jqGridParams.page, jqGridParams.rows),
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


            return Json("", JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}