import { Component, OnInit  } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { SearchRuleService } from './service/search-rule.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
@Component({
    selector: 'search-rule',
    templateUrl: './search-rule.component.html',
    providers: [SearchRuleService]
})
export class SearchRuleComponent implements OnInit{
    rule: SearchCriteria[];
    public onDelete: Function;
    private siteId: number;

    constructor(private searchRuleService: SearchRuleService,
                private route: ActivatedRoute,
                private location: Location) {
    }

    ngOnInit() {
        this.route.params
            .switchMap((params: Params) => this.searchRuleService.getSearchRule(+params['id']))
            .subscribe(rule => this.rule = rule);
        this.route.params.subscribe(params => {
            this.siteId = +params['id']
        })
        this.onDelete = this.onDeleteClick.bind(this)

        //let response = this.searchRuleService.getSearchRule()
        //response.then(rule => {
        //    this.rule = rule
        //    this.onDelete = this.onDeleteClick.bind(this)
        //});
    }

    public onAddClick(): void {
        this.rule.push(new SearchCriteria());
    }

    public onSaveClick(): void {


        this.searchRuleService.saveSearchRule(this.rule, this.siteId)
        //this.searchRuleService.saveSearchRule(this.rule)
    }

    public onDeleteClick(index): void {
        this.rule.splice(index, 1)
    }
}