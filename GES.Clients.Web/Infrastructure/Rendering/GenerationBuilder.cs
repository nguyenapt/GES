﻿using GES.Inside.Data.Models;

namespace GES.Clients.Web.Infrastructure.Rendering
{
    public class GenerationBuilder : CaseProfileBuilder<CaseProfileGenerationViewModel>
    {
        public override CaseProfileBuilder BuildLeftView()
        {
            BuildTag(@"<div class='col-md-8 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "SrIssue", ViewModel.IssueComponent)
                .BuildComponent(Helper, "SrEngagementInformation", ViewModel.EngagementInformationComponent)
                .BuildComponent(Helper, "UNGP_AssessmentTabs", ViewModel.CaseProfileUNGPAssessmentMethodologyComponent)
                .BuildComponent(Helper, "CompanyAndSourceDialogueWithLogs", ViewModel.IssueComponent)
                .BuildComponent(Helper, "AdditionalIncidents", ViewModel)
                .BuildComponent(Helper, "Disclaimer", ViewModel)
                .BuildTag(@"</div>");

            return this;
        }

        public override CaseProfileBuilder BuilRightView()
        {
            BuildTag(@"<div class='col-md-4 col-sm-12 col-xs-12'>")
                .BuildComponent(Helper, "Base", ViewModel.BaseComponent)
                .BuildComponent(Helper, "SrCaseBase", ViewModel.CaseComponent)
                .BuildComponent(Helper, "GesContact", ViewModel.ContactEngagementManager)
                .BuildComponent(Helper, "Statistic", ViewModel.StatisticComponent)
                .BuildComponent(Helper, "Endorsement", ViewModel.Endorsement)
                .BuildComponent(Helper, "Calendar", ViewModel.Events)
                .BuildComponent(Helper, "SrSdg", ViewModel.Sdgs)
                .BuildComponent(Helper, "Document", ViewModel.AdditionalDocuments)
                .BuildTag(@"</div>");

            return this;
        }
    }
}