using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class SrEvaluateBurmaBuilder : CaseProfileBuilder<CaseProfileSrEvaluateViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")                
                .BuildComponent(Helper, "Issue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "LogAndDialougeReport", ViewModel)
                .BuildComponent(Helper, "AdditionalIncidents", ViewModel)
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
                .BuildComponent(Helper, "GesContact", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }
    }
}