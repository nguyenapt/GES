using System.Web.Mvc;
using GES.Inside.Data.Services.Interfaces;
using System;
using GES.Clients.Web.Extensions;
using GES.Common.Exceptions;
using GES.Common.Logging;

namespace GES.Clients.Web.Controllers
{
    [Authorize]
    public class ReportController : GesControllerBase
    {
        private readonly IGesDocumentService _gesDocumentService;
        private readonly IGesFileStorageService _gesFileStorageService;
        private readonly IG_IndividualsService _gIndividualsService;

        public ReportController(IGesLogger logger, IGesDocumentService glossaryService,
            IGesFileStorageService gesFileStorageService, IG_IndividualsService gIndividualService)
            : base(logger)
        {
            _gesDocumentService = glossaryService;
            _gesFileStorageService = gesFileStorageService;
            _gIndividualsService = gIndividualService;
        }


        public ActionResult AnnualEngagement()
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var anualReports = _gesDocumentService.GetAnualReport(orgId);

                return View(anualReports);
            }
            catch(GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }

        public ActionResult QuarterlyEngagement()
        {            
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var quarterlyReports = this._gesDocumentService.GetQuarterlyReport(this.GetOrganizationId(_gIndividualsService));


                return View(quarterlyReports);
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }

        public ActionResult PositionPaper()
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var positionReports = this._gesDocumentService.GetPositionReport(this.GetOrganizationId(_gIndividualsService));
                
                return View(positionReports);
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }
        
        public ActionResult PositionPaperForConventions()
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);

            try
            {
                var positionReports = this._gesDocumentService.GetPositionReport(this.GetOrganizationId(_gIndividualsService));
                
                return View("PositionPaper",positionReports);
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, $"Error when getting the anual report of organization({orgId})");

                throw;
            }
        }

        [Route("Download/{documentId}")]
        public ActionResult Download(Guid? documentId)
        {
            if (!documentId.HasValue)
                return HttpNotFound();

            var orgId = this.GetOrganizationId(_gIndividualsService);

            var document = this.SafeExecute(() => this._gesDocumentService.GetDocumentById(orgId, documentId.Value), $"Exception when getting the document with criteria organization({orgId}) and document({documentId.Value})");

            if(document != null)
            {
                var fileStream = this.SafeExecute(() => this._gesFileStorageService.GetStream(orgId, documentId.Value), $"Exception when read the stream of document with criteria organization({orgId}) and document({documentId.Value})");

                if(fileStream != null && fileStream.CanRead)
                {
                    return File(fileStream, System.Web.MimeMapping.GetMimeMapping(document.FileName), document.FileName);
                } 
            }

            return HttpNotFound();
        }
    }
}