import { Component } from '@angular/core';
import { SearchCriteria } from './search-criteria';
@Component({
    selector: 'search-rule',
    templateUrl: './search-rule.component.html'
})
export class SearchRuleComponent {
    rule: Array<SearchCriteria> = [new SearchCriteria('City', 'equal', [{ id: 'Vancouver', text: 'Vancouver'}])];

    public onAddClick(): void {
        this.rule.push(new SearchCriteria());
    }
}