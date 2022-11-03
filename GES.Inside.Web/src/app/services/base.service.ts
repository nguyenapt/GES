import {HttpClient, HttpErrorResponse, HttpResponse} from "@angular/common/http";
import {Injectable} from "@angular/core";
import { filter, map, catchError } from 'rxjs/operators';
import {of} from "rxjs/internal/observable/of";

import {ToastrService} from "ngx-toastr";
import {Observable} from "../../../node_modules/@ngtools/webpack/node_modules/rxjs";

@Injectable({
    providedIn: 'root'
})
export class BaseService{

    constructor(public httpClient: HttpClient, public toastr: ToastrService) {
        
    }
    
    protected extractData(res: Response) {
        let body = res;
        return body || {};
    }


    protected handleError (error: Response | any) {
   
        let errMsg: string;
        if (error instanceof Response) {
            const body = error.json() || '';
            const err = JSON.stringify(body);
            errMsg = `${error.status} - ${error.statusText || ''} ${err}`;
        } else {
            errMsg = error.message ? error.message : error.toString();
        }
        console.log(errMsg);
               
       // this.toastr.success('failed: ' +  errMsg.toString(), 'Information');
        return of(error);
    }
}