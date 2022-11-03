import { Component, OnInit, Input } from '@angular/core';
import { GssIssueIndicator } from '../viewmodels/gss-issue-indicator';
import { GssPrincipleCompanyContact } from '../viewmodels/gss-principle-company-contact';

@Component({
    selector: 'app-gss-company-contact',
    templateUrl: './gss-company-contact.component.html',
    styleUrls: ['./gss-company-contact.component.css']
})
export class GssCompanyContactComponent implements OnInit {
    @Input() issueIndicator: GssIssueIndicator;
    constructor() { }

    ngOnInit() {
        if (this.issueIndicator.CompanyContact == null) {
            this.issueIndicator.CompanyContact = new GssPrincipleCompanyContact;
        }
    }

}
