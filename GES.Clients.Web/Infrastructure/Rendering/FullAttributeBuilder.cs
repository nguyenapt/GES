using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class FullAttributeBuilder : CaseProfileBuilder<CaseProfileFullAttributeViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")                
                .BuildComponent(Helper, $"{ViewModel.GetType().Name}/EngageOrDisengageIssue", ViewModel.IssueComponent)
                .BuildComponent(Helper, $"{ViewModel.GetType().Name}/EngagementInformationAndDiscussionAndStakeHolderTabs", ViewModel)
                .BuildComponent(Helper, "RevisionCriterialConfirmationReferencesTabs", ViewModel)
                .BuildComponent(Helper, "SrKpi", ViewModel.KpiViewModels)
                .BuildComponent(Helper, "UNGP_AssessmentTabs", ViewModel.CaseProfileUNGPAssessmentMethodologyComponent)
                .BuildComponent(Helper, "CompanyAndSourceDialogueWithLogsAndDownload", ViewModel.IssueComponent)                
                .BuildComponent(Helper, "AdditionalIncidents", ViewModel)                
                .BuildComponent(Helper, "RecomendationForChange", ViewModel)
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
                .BuildComponent(Helper, "Statistic", ViewModel.StatisticComponent)
                .BuildComponent(Helper, "Endorsement", ViewModel.Endorsement)
                .BuildComponent(Helper, "Calendar", ViewModel.Events)
                .BuildComponent(Helper, "SdgAndUngpAndConventionTabs", ViewModel.SdgAndGuidelineConventionComponent)
                .BuildComponent(Helper, "Document", ViewModel.AdditionalDocuments)                
                .BuildTag(@"</div>");

            return this;
        }
    }
}