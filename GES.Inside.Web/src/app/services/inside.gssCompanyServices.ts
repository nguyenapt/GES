import { HttpClient, HttpHeaders, HttpParams, HttpErrorResponse, HttpResponse } from "@angular/common/http";
import { GssCompanyResearch } from "../viewmodels/gss-company-research";
import { Observable } from "rxjs/internal/Observable";
import { AppSettings } from "../helper/appSettings";
import { map, catchError, tap } from 'rxjs/operators';
import { Injectable } from "@angular/core";
import { of } from "rxjs/internal/observable/of";
import { GssInternalComment } from '../viewmodels/gss-internal-comment';
import { BaseService } from "./base.service";
import { ToastrService } from "ngx-toastr";

const httpOptions = {
    headers: new HttpHeaders({
        'Content-Type': 'application/json'
    })
};

@Injectable({
    providedIn: 'root'
})
export class GssCompanyServices extends BaseService {

    constructor(httpClient: HttpClient, toastr: ToastrService) {
        super(httpClient, toastr);
    }

    getGssCompanyDetail(id: number): Observable<GssCompanyResearch> {
        let params = new HttpParams().set('id', id.toString());
      return this.httpClient.get<GssCompanyResearch>(AppSettings.GSS_COMPANY_DETAIL_API_ENDPOINT,
        { params: params }
      ).pipe(
        map((receivedData: GssCompanyResearch) => {
          return receivedData;

                    
        })           
        );
    }

    updateGssCompany(gssCompanyResearch): Observable<GssCompanyResearch> {
        console.log(gssCompanyResearch);
        return this.httpClient.post<any>(AppSettings.GSS_COMPANY_UPDATE_API_ENDPOINT, JSON.stringify(gssCompanyResearch), httpOptions).pipe(
            tap((gssCompanyResearch) => {


                this.toastr.success(`Success`, 'Information');
                console.log(`added GSS company/ id=${gssCompanyResearch.Id}`)
            }
            ),
            catchError(this.handleError)
        );
    }
    saveComment(comment): Observable<GssInternalComment> {
        console.log(comment);
        return this.httpClient.post<any>(AppSettings.GSS_COMPANY_SAVE_COMMENT_API_ENDPOINT, comment, httpOptions).pipe(
            tap((comment) => console.log(`added GSS Internal comment for / id=${comment.GssId}`)),
            catchError(this.handleError)
        );
    }
}
