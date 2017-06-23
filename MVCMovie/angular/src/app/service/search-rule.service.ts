import { Injectable } from '@angular/core';
import { SearchCriteria } from '../search-criteria'
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class SearchRuleService {
    private searchCriteriaUrl = '/api/searchrule/1'
    private baseUrl = '/api/searchrule/';
    private headers = new Headers({ 'Content-Type': 'application/json' });

    constructor(private http: Http) {

    }

    getSearchRule(id: number): Promise<SearchCriteria[]> {
        return this.http.get(this.baseUrl + id).toPromise().then(response => {
            let json = response.json();
            return json as SearchCriteria[]
        }
        ).catch(this.handleError);
    }

    saveSearchRule(searchRule : SearchCriteria[], id : number): Promise<SearchCriteria[]> {
        return this.http.post(this.baseUrl + id, JSON.stringify(searchRule), { headers: this.headers })
            .toPromise()
            .then(() => searchRule)
            .catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occcured', error);
        return Promise.reject(error.message || error);
    }
}