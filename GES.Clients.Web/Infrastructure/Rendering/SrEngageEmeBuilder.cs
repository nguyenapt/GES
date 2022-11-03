using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class SrEngageEmeBuilder : CaseProfileBuilder<CaseProfileSrEngageEmeViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")
                //.BuildComponent(Helper, "Timeline", string.Empty)
                .BuildComponent(Helper, "Issue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "UNGP_AssessmentTabs", ViewModel.CaseProfileUNGPAssessmentMethodologyComponent)
                .BuildComponent(Helper, "LogAndDialougeReport", ViewModel)
                .BuildComponent(Helper, "OtherInvestor", ViewModel)
                .BuildComponent(Helper, "AdditionalIncidents", ViewModel)
                .BuildComponent(Helper, "EngagementInformation", ViewModel)
                .BuildComponent(Helper, "Endorsement", ViewModel)
                .BuildComponent(Helper, "RecomendationForChange", ViewModel)
                .BuildComponent(Helper, "Disclaimer", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }

        public override CaseProfileBuilder BuilRightView()
        {
            BuildTag(@"<div class='col-md-4 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "Base", ViewModel.BaseComponent)
                .BuildComponent(Helper, "Case", ViewModel.CaseComponent)
                .BuildComponent(Helper, "Calendar", ViewModel)
                .BuildComponent(Helper, "Statistic", ViewModel)
                .BuildComponent(Helper, "Status", ViewModel)
                .BuildComponent(Helper, "GesContact", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }
    }
}
