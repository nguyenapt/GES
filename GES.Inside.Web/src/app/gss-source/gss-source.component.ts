import { Component, Input } from '@angular/core';
import { GssResearchDetailService } from '../services/gss-research-detail.service';
import { GssCompanyResearch } from '../viewmodels/gss-company-research';
import { GssSourceDialogComponent } from '../gss-source-dialog/gss-source-dialog.component';
import { MatDialog } from '@angular/material';
import { Helper } from '../helper/Helper';

@Component({
    selector: 'app-gss-source',
    templateUrl: './gss-source.component.html',
    styleUrls: ['./gss-source.component.css']    

})
export class GssSourceComponent {
    @Input() gssCompanyResearch: GssCompanyResearch;
    public sources;    
    
    constructor(private gssResearchDetailService: GssResearchDetailService, private dialog: MatDialog) { }

    ngOnInit() {
        this.getSources();
    }

    public getSources() {
        this.gssResearchDetailService.getSources()
            .subscribe(data => {
                this.sources = data;
            });
    }
    ReviveDateTime(value) {
        return Helper.ReviveDateTime(value);
    }
    addSource() {
        this.dialog.open(GssSourceDialogComponent, { disableClose: true });
    }
}
