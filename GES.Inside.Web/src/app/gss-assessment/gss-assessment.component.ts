import { Component, OnInit,Input } from '@angular/core';
import { GssIssueIndicator } from '../viewmodels/gss-issue-indicator';
import { ActivatedRoute, Router } from "@angular/router";
import { GssCompanyServices } from "../services/inside.gssCompanyServices";
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material';
import { GssAssessmentUngpBusinessHumanRightDialogComponent } from '../gss-assessment-ungp-business-human-right-dialog/gss-assessment-ungp-business-human-right-dialog.component';
import { GssAssessmentOtherRelatedConventionDialogComponent } from '../gss-assessment-other-related-convention-dialog/gss-assessment-other-related-convention-dialog.component';
import { GssAssessmentOcedDialogComponent } from '../gss-assessment-oced-dialog/gss-assessment-oced-dialog.component';

@Component({
    selector: 'app-gss-assessment',
    templateUrl: './gss-assessment.component.html',
    styleUrls: ['./gss-assessment.component.css']
})
export class GssAssessmentComponent implements OnInit {
    @Input() issueIndicator: GssIssueIndicator;

    isEdit = true;
    isEditAssessment = false;
    isEditBasisForNonCompliance = false;
    isEditOtherRelatedConvention = false;
    selected = "-1";
    years = [];
    currentYear: number;
    currentQuarter: number;

    constructor(private router: Router,
        private activatedRoute: ActivatedRoute,
        private gssCompanyServices: GssCompanyServices,
        private toastr: ToastrService,
        private dialog: MatDialog) { }

    ngOnInit() {
        this.rebuildForm();
    }

    rebuildForm() {
        var currentTime = new Date();
        this.currentYear = currentTime.getFullYear();
        this.currentQuarter = this.getQuarter(currentTime);

        for (var _i = 2005; _i <= this.currentYear; _i++) {
            this.years.push(_i);
        }

        if (this.selected == "0" || this.selected == "1" || this.selected == "2") {
            this.isEditAssessment = true;

            if (this.selected == "2") {
                this.isEditBasisForNonCompliance = true;
            }
            else {
                this.isEditBasisForNonCompliance = false;
            }

            if (this.selected == "1" || this.selected == "2") {
                this.isEditOtherRelatedConvention = true;
            }
            else {
                this.isEditOtherRelatedConvention = false;
            }
        }
        else {
            this.isEditAssessment = false;
        }        
    }
    selectUNGlobal() {        
        this.rebuildForm();
    }

    getQuarter(d) {
        var d = d || new Date();
        var m = Math.floor(d.getMonth() / 3) + 2;
        return m > 4 ? m - 4 : m;
    }

    openOCEDGuideline() {
        this.dialog.open(GssAssessmentOcedDialogComponent, {
            disableClose: true,
            data: []
        });
    }

    openUNGPBusinessHumanRight() {
        this.dialog.open(GssAssessmentUngpBusinessHumanRightDialogComponent, {
            disableClose: true,
            data: []
        });
    }

    openOtherRelatedConventions() {
        this.dialog.open(GssAssessmentOtherRelatedConventionDialogComponent, {
            disableClose: true,
            data: []
        });
    }
}
