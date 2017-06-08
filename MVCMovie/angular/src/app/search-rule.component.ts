import { Component, OnInit  } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { SearchRuleService } from './service/search-rule.service';
@Component({
    selector: 'search-rule',
    templateUrl: './search-rule.component.html',
    providers: [SearchRuleService]
})
export class SearchRuleComponent {
    rule: SearchCriteria[];
    public onDelete: Function;

    constructor(private searchRuleService: SearchRuleService) {
    }

    ngOnInit() {
        let response = this.searchRuleService.getSearchRule()
        response.then(rule => {
            this.rule = rule
            this.onDelete = this.onDeleteClick.bind(this)
        });
    }

    public onAddClick(): void {
        this.rule.push(new SearchCriteria());
    }

    public onDeleteClick(index): void {
        this.rule.splice(index, 1)
    }
}