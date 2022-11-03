import { Component } from '@angular/core';

@Component({
    selector: 'app-gss-assessment-ungp-business-human-right-dialog',
    templateUrl: './gss-assessment-ungp-business-human-right-dialog.component.html',
    styleUrls: ['./gss-assessment-ungp-business-human-right-dialog.component.css']
})
export class GssAssessmentUngpBusinessHumanRightDialogComponent {
    ungpBusinessHumanRights = [
        { text: '11 Respect for Human Rights', value: 1 },
        { text: '12 Respect Int. recognized Human Rights', value: 2 },
        { text: '13 Avoid / Prevent Human Rights impacts', value: 3 },
        { text: '14 Enterprise context and structure adverse impacts', value: 4 },
        { text: '15 Policy Commitment', value: 5 },
        { text: '16 Policy Statement', value: 6 },
        { text: '17 Human Rights Due Diligence', value: 7 },
        { text: '18 Identify and assess actual / potential impacts', value: 8 },
        { text: '19 Integration and appropriate action', value: 9 },
        { text: '20 Verification of adverse impacts with stakeholders', value: 10 },
        { text: '21 Communicate on human rights impacts', value: 11 },
        { text: '22 Provide remediation', value: 12 }
    ]
    selectedUNGPs: []
    lastSelectedUNGPs: [];

    //onSelectedUNGPChange(event: Event) {        
    //    if (this.selectedUNGPs && this.selectedUNGPs.length > 1 && this.lastSelectedUNGPs && this.lastSelectedUNGPs.length > 0) {
    //        delete this.selectedUNGPs[this.selectedUNGPs.indexOf(this.lastSelectedUNGPs[0])];
    //        this.lastSelectedUNGPs = this.selectedUNGPs;
    //    }        
    //}

    saveUNGPBusinessHumanRight() {
        var x = this.selectedUNGPs;
    }
}
