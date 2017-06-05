import { Component, OnInit  } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { SearchRuleService } from './service/search-rule.service';
@Component({
    selector: 'search-rule',
    templateUrl: './search-rule.component.html',
    providers: [SearchRuleService]
})
export class SearchRuleComponent {
    rule: Array<SearchCriteria>;

    constructor(private searchRuleService: SearchRuleService) {
    }

    ngOnInit() {
        this.searchRuleService.getSearchRule().then(rule => this.rule = rule);
    }

    public onAddClick(): void {
        this.rule.push(new SearchCriteria());
    }
}