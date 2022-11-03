import { Component, OnInit, Input } from '@angular/core';
import { GssResearchDetailService } from '../services/gss-research-detail.service';
import { GssCompanyResearch } from '../viewmodels/gss-company-research';
import { MatDialog } from '@angular/material';
import { GssInternalCommentDialogComponent } from '../gss-internal-comment-dialog/gss-internal-comment-dialog.component';
import { GssInternalComment } from '../viewmodels/gss-internal-comment';
import { Helper } from '../helper/Helper';

@Component({
  selector: 'app-gss-internal-comment',
  templateUrl: './gss-internal-comment.component.html',
  styleUrls: ['./gss-internal-comment.component.css']
})
export class GssInternalCommentComponent implements OnInit {
    @Input() gssCompanyResearch: GssCompanyResearch;
    
    public filteredInternalComments;
    

    constructor(private gssResearchDetailService: GssResearchDetailService, private dialog: MatDialog) { }

    ngOnInit() {
        this.assignFilteredInternalComment();
    }

    public assignFilteredInternalComment() {
        if (this.gssCompanyResearch != null && this.gssCompanyResearch != undefined) {
            this.filteredInternalComments = Object.assign([], this.gssCompanyResearch.GssInternalCommentViewModels);
        }
    }

    public filterItem(value) {
        if (value == "-1") {
            this.assignFilteredInternalComment();
        }
        else {
            this.filteredInternalComments = Object.assign([], this.gssCompanyResearch.GssInternalCommentViewModels).filter(
                item => item.Level.toString() == value
            )
        }
    } 
    ReviveDateTime(value) {
        return Helper.ReviveDateTime(value);
    }
    addComment() {
        let comment = new GssInternalComment();
        comment.Id = 0;
        comment.GssId = 1;
        comment.DateModified = new Date().toString();
        comment.User_Id = 1;
        comment.UserName = "";
        comment.Level = -1;
        comment.Comment = "";

        this.dialog.open(GssInternalCommentDialogComponent, {
            disableClose: true,
            data: comment
        });
    }
}
