import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { GssSource } from '../viewmodels/gss-source';
import { GssInternalComment } from '../viewmodels/gss-internal-comment';
import { Helper } from '../helper/Helper';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class GssResearchDetailService {

    constructor(private httpClient: HttpClient) { }

    private handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {

            // TODO: send the error to remote logging infrastructure
            console.error(error); // log to console instead

            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
    //Sources
    getSources(): Observable<GssSource[]> {
        return this.httpClient.get(`/GSSResearch/GetGssSources`).
            pipe(
            map((item: any) => item.map(p => <GssSource>
                {
                    Id: p.Id,
                    GssId: p.GssId,
                    DateModified: Helper.ReviveDateTime(p.DateModified),
                    Description: p.Description
                })));
    }

    getSource(id: string): Observable<GssSource> {           
        return this.httpClient.get<GssSource>("/GSSResearch/GetGssSource", httpOptions).pipe(
            tap(_ => console.log(`fetched Source id=${id}`)),
            catchError(this.handleError<GssSource>(`GetGssSource id=${id}`))
        );
    }    

    saveSource(source: any): Observable<any> {        
        return this.httpClient.put("/GSSResearch/SaveGssSources", source, httpOptions).pipe(
            tap(_ => console.log(`updated Source id=${source.Id}`)),
            catchError(this.handleError<any>('saveSource'))
        );
    }

    deleteSource(id: string): Observable<GssSource> {        
        return this.httpClient.delete<GssSource>("/GSSResearch/DeleteGssSource", httpOptions).pipe(
            tap(_ => console.log(`deleted Gss Source id=${id}`)),
            catchError(this.handleError<GssSource>('DeleteGssSource'))
        );
    }

    //End Sources

    //Internal Comment
    getInternalComments(): Observable<GssInternalComment[]> {
        return this.httpClient.get(`/GSSResearch/GetGssInternalComments`).
            pipe(
            map((item: any) => item.map(p => <GssInternalComment>
                    {
                        Id: p.Id,
                        GssId: p.GssId,
                        UserName: p.UserName,
                        DateModified: Helper.ReviveDateTime(p.DateModified),
                        Comment: p.Comment
                    })));
    }

    getInternalComment(id: string): Observable<GssInternalComment> {
        return this.httpClient.get<GssInternalComment>("/GSSResearch/GetGssInternalComment", httpOptions).pipe(
            tap(_ => console.log(`fetched GssInternalComment id=${id}`)),
            catchError(this.handleError<GssInternalComment>(`GssInternalComment id=${id}`))
        );
    }

    saveInternalComment(source: any): Observable<any> {
        return this.httpClient.put("/GSSResearch/SaveGssInternalComment", source, httpOptions).pipe(
            tap(_ => console.log(`updated GssInternalComment id=${source.Id}`)),
            catchError(this.handleError<any>('saveInternalComment'))
        );
    }

    deleteInternalComment(id: string): Observable<GssInternalComment> {
        return this.httpClient.delete<GssInternalComment>("/GSSResearch/DeleteGssInternalComments", httpOptions).pipe(
            tap(_ => console.log(`deleted GssInternalComment id=${id}`)),
            catchError(this.handleError<GssInternalComment>('GssInternalComment'))
        );
    }

    //End Internal Comment
}
