using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class BcEvaluateBuilder : CaseProfileBuilder<CaseProfileBcEvaluateViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")                
                .BuildComponent(Helper, "Issue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "EvaluateRevisionCriterialConfirmationTabs", ViewModel)
                .BuildComponent(Helper, "CompanyAndSourceDialogueWithLogsAndDownload", ViewModel.IssueComponent)
                .BuildComponent(Helper, "AdditionalIncidents", ViewModel)
                .BuildComponent(Helper, "Disclaimer", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }

        public override CaseProfileBuilder BuilRightView()
        {
            BuildTag(@"<div class='col-md-4 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "Base", ViewModel.BaseComponent)
                .BuildComponent(Helper, $"{ViewModel.GetType().Name}/Case", ViewModel.CaseComponent)
                .BuildComponent(Helper, "GesContact", ViewModel.ContactEngagementManager)
                .BuildComponent(Helper, "Endorsement", ViewModel.Endorsement)
                .BuildComponent(Helper, "Calendar", ViewModel.Events)
                .BuildComponent(Helper, "GuidelineTab", ViewModel.IssueComponent?.Guidelines)
                .BuildComponent(Helper, "Document", ViewModel.AdditionalDocuments)
                .BuildTag(@"</div>");

            return this;
        }
    }
}
