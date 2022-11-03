import { Component, OnInit, Input, Inject  } from '@angular/core';
import { GssCompanyServices } from '../services/inside.gssCompanyServices';
import { GssCompanyResearch } from '../viewmodels/gss-company-research';
import { Router } from '@angular/router';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { GssInternalComment } from '../viewmodels/gss-internal-comment';

@Component({
    selector: 'app-gss-internal-comment-dialog',
    templateUrl: './gss-internal-comment-dialog.component.html',
    styleUrls: ['./gss-internal-comment-dialog.component.css']
})
export class GssInternalCommentDialogComponent implements OnInit {
    @Input() gssCompanyResearch: GssCompanyResearch;
    comment: GssInternalComment;
    sections = [{ text: 'Select Section', value: -1 }, { text: 'Company Level', value: 0 }, { text: 'Department Level', value: 1 }, { text: 'Staff Level', value: 2 }]

    constructor(private router: Router, private gssCompanyServices: GssCompanyServices, @Inject(MAT_DIALOG_DATA) private data) { }

    ngOnInit() {
        this.comment = new GssInternalComment()
        this.comment.Id = this.data.Id;
        this.comment.GssId = this.data.GssId;
        this.comment.DateModified = this.data.DateModified;
        this.comment.User_Id = this.data.User_Id;
        this.comment.UserName = this.data.UserName;
        this.comment.Level = this.data.Level;
        this.comment.Comment = this.data.Comment;
    }

    saveComment() {
        this.gssCompanyServices.saveComment(this.comment).subscribe((result) => {
            this.router.navigate(['/CompanyDetails/' + result.Id]);
        }, (err) => {
            console.log(err);
        });
    
    }    
}
