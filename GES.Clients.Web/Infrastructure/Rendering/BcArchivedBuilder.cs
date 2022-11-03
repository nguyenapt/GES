using GES.Common.Resources;
using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class BcArchivedBuilder : CaseProfileBuilder<CaseProfileBcArchivedViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {            
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")                
                .BuildComponent(Helper, "Issue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "RevisionCriterialConfirmationReferencesTabs", ViewModel)
                .BuildComponent(Helper, "DiscussionPoint", ViewModel.DiscussionPoints)
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
                .BuildComponent(Helper, "BcCaseBase", ViewModel.CaseComponent)
                .BuildComponent(Helper, "GesContact", ViewModel.ContactEngagementManager)
                .BuildComponent(Helper, "GuidelineTab", ViewModel.IssueComponent?.Guidelines)
                .BuildComponent(Helper, "Document", ViewModel.AdditionalDocuments)
                .BuildTag(@"</div>");

            return this;
        }
    }
}