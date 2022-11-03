using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using GES.Clients.Web.Models;
using GES.Inside.Data.DataContexts;
using GES.Inside.Data.Services.Interfaces;
using Z.EntityFramework.Plus;
using GES.Common.Logging;
using GES.Common.Exceptions;
using GES.Clients.Web.Extensions;

namespace GES.Clients.Web.Controllers
{
    public class CaseProfileController : GesControllerBase
    {
        #region Declaration
        private readonly II_GesCaseProfilesService _gesCaseProfilesService;
        private readonly IG_IndividualsService _gIndividualsService;        
        #endregion

        #region Constructor
        public CaseProfileController(IGesLogger logger, II_GesCaseProfilesService gesCaseProfilesService, IG_IndividualsService gIndividualsService)
            : base(logger)
        {
            _gesCaseProfilesService = gesCaseProfilesService;
            _gIndividualsService = gIndividualsService;
        }

        #endregion

        #region ActionResult

        public ActionResult CreateMyEndorsementForm(long caseReportId)
        {
            var orgId = this.GetOrganizationId(_gIndividualsService);
            var activiForm = this.SafeExecute(() => _gesCaseProfilesService.GetActivityForm(caseReportId, orgId),
                $"Exception when get the activity form with caseReportId:{caseReportId} and organizationId:{orgId}");

            var viewModel = new MyEndorsementViewModel
            {
                ActivityFormId = -1,
                EngagementActivityOptions = this.SafeExecute(() => _gesCaseProfilesService.GetActivityOptions(), $"Exception when get activity options")
                                                                      .Select(i => new SelectListItem
                                                                      {
                                                                          Value = i.I_EngagementActivityOptions_Id.ToString(),
                                                                          Text = i.Name
                                                                      })
            };

            if (activiForm == null)
            {
                viewModel.GesCaseReportId = caseReportId;
                viewModel.OrgId = orgId;
                return PartialView("_MyEndorsement", viewModel);
            }

            viewModel.ActivityFormId = activiForm.I_ActivityForms_Id;
            viewModel.EngagementActivityOption = activiForm.I_EngagementActivityOptions_Id;
            viewModel.Ownership = activiForm.Ownership.HasValue && activiForm.Ownership.Value;
            viewModel.NumberofShareCount = activiForm.Shares?.ToString() ?? string.Empty;
            viewModel.NumberofShareDate = activiForm.AsOf?.ToString("MM/dd/yyyy") ?? string.Empty;
            viewModel.HoldingsThroughExternalFunds = activiForm.HoldingsThroughExternalFunds.HasValue && activiForm.HoldingsThroughExternalFunds.Value;
            viewModel.FixedIncomeHoldings = activiForm.FixedIncomeHoldings.HasValue && activiForm.FixedIncomeHoldings.Value;
            viewModel.SignLetters = activiForm.SignLetters.HasValue && activiForm.SignLetters.Value;
            viewModel.ParticipateInConferenceCalls = activiForm.ParticipateInConferenceCalls.HasValue && activiForm.ParticipateInConferenceCalls.Value;
            viewModel.ParticipateInLiveMeetings = activiForm.ParticipateInLiveMeetings.HasValue && activiForm.ParticipateInLiveMeetings.Value;
            viewModel.FileorCoFileResolutions = activiForm.FileResolutions.HasValue && activiForm.FileResolutions.Value;
            viewModel.CleaningHouseActions = activiForm.ClearinghouseActions.HasValue && activiForm.ClearinghouseActions.Value;
            viewModel.QuestionToAgms = activiForm.QuestionsToAgms.HasValue && activiForm.QuestionsToAgms.Value;
            viewModel.CollaborativeActions = activiForm.CollaborativeActions.HasValue && activiForm.CollaborativeActions.Value;
            viewModel.Suggestions = activiForm.Suggestions;
            viewModel.GesCaseReportId = activiForm.I_GesCaseReports_Id;
            viewModel.OrgId = activiForm.G_Organizations_Id;

            return PartialView("_MyEndorsement", viewModel);
        }

        public JsonResult UpdateEndorsement(MyEndorsementViewModel viewModel)
        {
            var activityForm = new I_ActivityForms()
                               {
                                   I_ActivityForms_Id = viewModel.ActivityFormId,
                                   I_EngagementActivityOptions_Id = viewModel.EngagementActivityOption,
                                   Ownership = viewModel.Ownership,
                                   Shares = !string.IsNullOrEmpty(viewModel.NumberofShareCount)
                                                ? int.Parse(viewModel.NumberofShareCount)
                                                : 0,
                                   AsOf = !string.IsNullOrEmpty(viewModel.NumberofShareDate)
                                              ? DateTime.ParseExact(viewModel.NumberofShareDate, "MM/dd/yyyy", CultureInfo.InvariantCulture)
                                              : (DateTime?) null,
                                   HoldingsThroughExternalFunds = viewModel.HoldingsThroughExternalFunds,
                                   FixedIncomeHoldings = viewModel.FixedIncomeHoldings,
                                   SignLetters = viewModel.SignLetters,
                                   ParticipateInConferenceCalls = viewModel.ParticipateInConferenceCalls,
                                   ParticipateInLiveMeetings = viewModel.ParticipateInLiveMeetings,
                                   FileResolutions = viewModel.FileorCoFileResolutions,
                                   ClearinghouseActions = viewModel.CleaningHouseActions,
                                   QuestionsToAgms = viewModel.QuestionToAgms,
                                   CollaborativeActions = viewModel.CollaborativeActions,
                                   Suggestions = viewModel.Suggestions,
                                   Created = DateTime.UtcNow,
                                   G_Organizations_Id = viewModel.OrgId,
                                   I_GesCaseReports_Id = viewModel.GesCaseReportId,                                   
                               };

            try
            {
                var newObject = _gesCaseProfilesService.UpdateEndorsement(activityForm);

                QueryCacheManager.ExpireTag("gescasereport");

                return newObject == null
                           ? Json(new
                           {
                               success = false,
                           })
                           : Json(new
                           {
                               success = true,
                           });
            }
            catch (GesServiceException ex)
            {
                Logger.Error(ex, "Error when update the activity form {@I_ActivityForms}", activityForm);

                throw;
            }
        }

        #endregion
    }
}