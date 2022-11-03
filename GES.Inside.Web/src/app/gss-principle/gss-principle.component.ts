import { Component, OnInit, Input, Inject } from '@angular/core';
import { GssCompanyServices } from '../services/inside.gssCompanyServices';
import { GssCompanyResearch } from '../viewmodels/gss-company-research';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { GssPrinciple } from '../viewmodels/gss-principle';
import { GssPrincipleAssessment } from "../viewmodels/gss-principle-assessment";
import { GssPrincipleGeneral } from "../viewmodels/gss-principle-general";
import { GssPrincipleUpgradeCriteria } from "../viewmodels/gss-principle-upgrade-criteria";
import { GssPrincipleCompanyContact } from "../viewmodels/gss-principle-company-contact";
import { GssIssueIndicator } from '../viewmodels/gss-issue-indicator';

@Component({
    selector: 'app-gss-principle',
    templateUrl: './gss-principle.component.html',
    styleUrls: ['./gss-principle.component.css']
})
export class GssPrincipleComponent implements OnInit {
    @Input() issueIndicators: GssIssueIndicator[];
    @Input() principleGroup: number;

    public filteredIssueIndicators;

    constructor() { }

    ngOnInit() {
        this.filterIssueIndicatorsByGroup(this.principleGroup)
    }

    public filterIssueIndicatorsByGroup(value) {
        this.filteredIssueIndicators = Object.assign([], this.issueIndicators).filter(
            item => item.Group == value
        )
    }
}
