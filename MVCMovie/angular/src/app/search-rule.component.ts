import { Component, OnInit, ChangeDetectorRef, AfterViewChecked, ViewChild } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { SearchRuleService } from './service/search-rule.service';
import { ActivatedRoute, Params } from '@angular/router';
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { NgForm, FormArray } from '@angular/forms'

@Component({
    selector: 'search-rule',
    templateUrl: './search-rule.component.html',
    providers: [SearchRuleService]
})
export class SearchRuleComponent implements OnInit{
    rule: SearchCriteria[];
    public onDelete: Function;
    private siteId: number;
    private provinces: string[];

    private fieldName: string;
    ruleForm: NgForm;
    @ViewChild('ruleForm') currentForm: NgForm;

    constructor(private searchRuleService: SearchRuleService,
                private route: ActivatedRoute,
                private location: Location,
                private cdRef: ChangeDetectorRef) {
    }

    ngOnInit() {
        this.route.params
            .switchMap((params: Params) => this.searchRuleService.getSearchRule(+params['id']))
            .subscribe(rule => this.rule = rule);
        this.route.params.subscribe(params => {
            this.siteId = +params['id']
        })
        this.onDelete = this.onDeleteClick.bind(this)

        this.fieldName = "test"
    }

    ngAfterViewChecked() {
        this.formChanged();
        //Set province if there is province criteria
        if (this.rule != null) {
            this.rule.map((criteira) => {
                if (criteira.fieldName === 'Province') {
                    this.provinces = criteira.values
                    this.cdRef.detectChanges();
                }
            })
        }
    }

    formChanged() {
        if (this.currentForm === this.ruleForm) { return; }
        this.ruleForm = this.currentForm;
        if (this.ruleForm) {
            this.ruleForm.valueChanges
                .subscribe(data => this.onValueChanged(data));
        }
    }

    onValueChanged(data?: any) {
        if (!this.ruleForm) { return; }
        const form = this.ruleForm.form;

        for (const field in this.formErrors) {
            // clear previous error message (if any)
            this.formErrors[field] = '';
            const control = form.get(field);

            if (control && control.dirty && !control.valid) {
                const messages = this.validationMessages[field];
                for (const key in control.errors) {
                    this.formErrors[field] += messages[key] + ' ';
                }
            }
        }
    }

    formErrors = {
        'fieldName': ''
    };

    validationMessages = {
        'fieldName': {
            'required': 'Name is required.',
            'minlength': 'Name must be at least 10 characters long.'
        }
    };

    public onAddClick(): void {
        this.rule.push(new SearchCriteria());
    }

    public onSaveClick(rule: FormArray): void {
        console.log(rule, this.currentForm)
        this.searchRuleService.saveSearchRule(this.rule, this.siteId)
        //this.searchRuleService.saveSearchRule(this.rule)
    }

    public onDeleteClick(index): void {
        if (this.rule[index].fieldName === 'Province') {
            this.provinces = null
        }
        this.rule.splice(index, 1)
    }
}