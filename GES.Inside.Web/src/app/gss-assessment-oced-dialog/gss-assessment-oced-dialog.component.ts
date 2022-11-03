import { Component } from '@angular/core';

@Component({
    selector: 'app-gss-assessment-oced-dialog',
    templateUrl: './gss-assessment-oced-dialog.component.html',
    styleUrls: ['./gss-assessment-oced-dialog.component.css']
})
export class GssAssessmentOcedDialogComponent {
    ocedGuidelines = [
        { text: 'Chapter IV - Human Rights', value: 1 },
        { text: 'Chapter V - Employment and Industrial Relations', value: 2 },
        { text: 'Chapter VI - Environment', value: 3 },
        { text: 'Chapter VII - Combating Bribery, Bribe Solicitation and Extortion', value: 4 },
        { text: 'Chapter VIII - Consumer Interests', value: 5 },
        { text: 'Chapter IX - Science and Technology', value: 6 },
        { text: 'Chapter X - Competition', value: 7 }
    ]

    selectedOCEDGuidelines: []

    saveOCEDGuideline() {
        var x = this.selectedOCEDGuidelines;
    }
}
