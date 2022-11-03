using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class StResolvedBuilder : CaseProfileBuilder<CaseProfileStandardViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "StResolvedIssue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "StRevisionCriterialConfirmationReferencesTabs", ViewModel)
                .BuildComponent(Helper, "CompanyAndSourceDialogueWithLogs", ViewModel.IssueComponent)
                .BuildComponent(Helper, "StAdditionalIncidents", ViewModel)
                .BuildComponent(Helper, "Disclaimer", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }

        public override CaseProfileBuilder BuilRightView()
        {
            BuildTag(@"<div class='col-md-4 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "StBase", ViewModel.BaseComponent)
                .BuildComponent(Helper, "StCaseBase", ViewModel.CaseComponent)
                .BuildComponent(Helper, "StGuidelineAndConvention", ViewModel.GuidelineConventionComponent)
                .BuildTag(@"</div>");

            return this;
        }
    }
}