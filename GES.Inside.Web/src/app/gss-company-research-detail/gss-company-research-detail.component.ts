import { Component, OnInit, ViewChild } from '@angular/core';
import { GssCompanyResearch } from "../viewmodels/gss-company-research";
import { CommentDetailModel } from "../viewmodels/comment-details-model";
import { GssCompanyServices } from "../services/inside.gssCompanyServices";
import { ActivatedRoute, Router } from "@angular/router";
import { CommonLib } from "../helper/commonLib";
import { map } from "rxjs/operators";
import { GssSourceComponent } from "../gss-source/gss-source.component"
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-gss-company-research-detail',
    templateUrl: './gss-company-research-detail.component.html',
    styleUrls: ['./gss-company-research-detail.component.css']
})
export class GssCompanyResearchDetailComponent implements OnInit {
    @ViewChild(GssSourceComponent, null) gssSourceComponent;

    gssCompanyResearch: GssCompanyResearch;
    edittingInternalAnalystComment: CommentDetailModel;
    edittingOutlookComment: CommentDetailModel;

    public companyId;
    public href: string = "";

    constructor(private router: Router, private activatedRoute: ActivatedRoute,
        private gssCompanyServices: GssCompanyServices,
        private toastr: ToastrService
    ) {
    }

    ngOnInit() {
        this.edittingInternalAnalystComment = new CommentDetailModel();
        let companyId = CommonLib.GetId(this.router.url);
        this.loadCompanyDetail(companyId);
    }

    public loadCompanyDetail(id: number) {
        this.href = this.router.url;
        this.gssCompanyServices.getGssCompanyDetail(id).subscribe((data) => {

            this.gssCompanyResearch = data as GssCompanyResearch;

            if (this.gssCompanyResearch.InternalAnalystComments != null && this.gssCompanyResearch.InternalAnalystComments.length > 0) {
                this.edittingInternalAnalystComment = this.gssCompanyResearch.InternalAnalystComments[0];
            }

        });
    }

    public updateGssCompany() {
        this.gssCompanyServices.updateGssCompany(this.gssCompanyResearch).subscribe((result) => {
            this.router.navigate(['GSSResearch/CompanyDetails/' + result.Id]);
        }, (err) => {
            console.log(err);
        });
    }

    public setEdittingInternalAnalystComment(event, internalComment) {
        this.edittingInternalAnalystComment = internalComment;
    }


}

