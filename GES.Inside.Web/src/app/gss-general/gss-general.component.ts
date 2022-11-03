import { Component, OnInit, Input } from '@angular/core';
import { GssIssueIndicator } from '../viewmodels/gss-issue-indicator';
import { GssPrincipleGeneral } from '../viewmodels/gss-principle-general';
import { GssPrincipleGeneralItem } from '../viewmodels/gss-principle-general-item';
import { Helper } from '../helper/Helper';

@Component({
    selector: 'app-gss-general',
    templateUrl: './gss-general.component.html',
    styleUrls: ['./gss-general.component.css']
})
export class GssGeneralComponent implements OnInit {
    @Input() issueIndicator: GssIssueIndicator;
    
    currentHeadline: GssPrincipleGeneralItem;
    currentCase: GssPrincipleGeneralItem;
    currentImpact: GssPrincipleGeneralItem;
    currentImpactAssessment: GssPrincipleGeneralItem;
    constructor() { }

    ngOnInit() {
        if (this.issueIndicator.General == null) {
            this.issueIndicator.General = new GssPrincipleGeneral;
        }
        if (this.issueIndicator.General.HeadLineViewModels == null) {
            this.issueIndicator.General.HeadLineViewModels = new Array();
            this.currentHeadline = new GssPrincipleGeneralItem();
            this.currentHeadline.Description = "test currentHeadline";
            this.currentHeadline.DateModified = Helper.ReviveDateTime(new Date());
            this.issueIndicator.General.HeadLineViewModels.push(this.currentHeadline);
        }
        if (this.issueIndicator.General.CaseSummaryViewModels == null) {
            this.issueIndicator.General.CaseSummaryViewModels = new Array();
            this.currentCase = new GssPrincipleGeneralItem();
            this.currentCase.Description = "test currentCase";
            this.currentCase.DateModified = Helper.ReviveDateTime(new Date());
            this.issueIndicator.General.CaseSummaryViewModels.push(this.currentCase);
        }
        if (this.issueIndicator.General.ImpactViewModels == null) {
            this.issueIndicator.General.ImpactViewModels = new Array();
            this.currentImpact = new GssPrincipleGeneralItem();
            this.currentImpact.Description = "test currentImpact";
            this.currentImpact.DateModified = Helper.ReviveDateTime(new Date());
            this.issueIndicator.General.ImpactViewModels.push(this.currentImpact);
        }
        if (this.issueIndicator.General.ImpactAssessmentViewModels == null) {
            this.issueIndicator.General.ImpactAssessmentViewModels = new Array();
            this.currentImpactAssessment = new GssPrincipleGeneralItem();
            this.currentImpactAssessment.Description = "test currentImpactAssessment";
            this.currentImpactAssessment.DateModified = Helper.ReviveDateTime(new Date());
            this.issueIndicator.General.ImpactAssessmentViewModels.push(this.currentImpactAssessment);
        }
    }

}
