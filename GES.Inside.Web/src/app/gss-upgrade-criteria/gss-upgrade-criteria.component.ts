import { Component, OnInit, Input } from '@angular/core';
import { GssIssueIndicator} from '../viewmodels/gss-issue-indicator'
import { GssPrincipleUpgradeCriteria } from '../viewmodels/gss-principle-upgrade-criteria';

@Component({
    selector: 'app-gss-upgrade-criteria',
    templateUrl: './gss-upgrade-criteria.component.html',
    styleUrls: ['./gss-upgrade-criteria.component.css']
})
export class GssUpgradeCriteriaComponent implements OnInit {
    @Input() issueIndicator: GssIssueIndicator;

    constructor() { }

    ngOnInit() {
        if (this.issueIndicator.UpgradeCriteria == null) {
            this.issueIndicator.UpgradeCriteria = new GssPrincipleUpgradeCriteria;
        }
    }

}
