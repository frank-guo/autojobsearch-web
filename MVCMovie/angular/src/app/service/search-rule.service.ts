import { Injectable } from '@angular/core';
import { SearchCriteria } from '../search-criteria'
import { Headers, Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class SearchRuleService {
    private searchCriteriaUrl = '/api/searchrule/1'

    constructor(private http: Http) {

    }

    getSearchRule(): Promise<SearchCriteria[]> {
        //return Promise.resolve([new SearchCriteria('City', 'equal', [{ id: 'Vancouver', text: 'Vancouver' }])]);
        return this.http.get(this.searchCriteriaUrl).toPromise().then(response => {
            let json = response.json();
            return json as SearchCriteria[]
        }
        ).catch(this.handleError);
    }

    private handleError(error: any): Promise<any> {
        console.error('An error occcured', error);
        return Promise.reject(error.message || error);
    }
}