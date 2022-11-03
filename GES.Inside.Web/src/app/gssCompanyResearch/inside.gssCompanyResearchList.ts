import { Component, OnInit } from '@angular/core';

@Component({
    selector: 'inside-gss-company-research-list',
    templateUrl: './inside.gssCompanyResearchList.html',
    styleUrls: ['./inside.gssCompanyResearchList.css']
})
export class GssCompanyResearchList implements OnInit {
    public gssCompanyResearchList;
    ngOnInit() {
        this.loadGssCompanyResearchList();
    }

    public loadGssCompanyResearchList() {
        
    }
    
    public initGrid(){
        
    }
}