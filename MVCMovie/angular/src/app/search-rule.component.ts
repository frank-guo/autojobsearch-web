import { Component, OnInit, ChangeDetectorRef, AfterViewChecked, ViewChild, SimpleChanges } from '@angular/core';
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
export class SearchRuleComponent implements OnInit {
    rule: SearchCriteria[];
    public onDelete: Function;
    private siteId: number;
    private provinces: string[];

    formValue: SearchCriteria;
    @ViewChild('ruleForm') currentForm: NgForm;
    private prevFieldName: any[];
    private prevOperator: any[];

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
    }

    ngAfterViewChecked() {
        this.formChanged();
        //Set province if there is province criteria
        if (this.rule != null) {
            this.rule.map((criteira) => {
                if (criteira.fieldName === ['Province']) {
                    this.provinces = criteira.values
                    this.cdRef.detectChanges();
                }
            })
        }
    }

    //ngDoCheck() {
    //    //This method can be called even though the rule reference is not changed when the inside property of the rule changes 
    //    console.log("rule=", this.rule)
    //}

    private equal(array1: string[], array2: string[]) : boolean{
        if (array1 == null || array2 == null || array1.length !== array2.length) {
            return false
        } else {
            for (let i = 0; i < array1.length; i++) {
                if (array1[i] !== array2[i]) {
                    return false
                }
            }
            return true
        }
    }

    formChanged() {
        if (this.currentForm == null || this.currentForm.value === this.formValue) { return; }
        //let value = Object.assign({}, this.currentForm.value)
        //let rule0 = Object.assign({}, value.criteria0)
        //value.criteria0 = rule0
        //console.log(value)
        let value = this.currentForm.value
        let criteria0 = Object.assign({}, value.criteria0)
        if (criteria0 != null) {
            let fieldName = criteria0.fieldName
            let _operator = criteria0._operator

            if (fieldName != null && !this.equal(this.prevFieldName, fieldName) || 
                _operator != null && !this.equal(this.prevOperator, _operator)) {
                let newFN = []
                if (fieldName != null && fieldName.length > 0) {
                    for (let field of fieldName) {
                        newFN.push(field)
                    }
                }
                //criteria0.fieldName = newFN
                //value.criteria0 = criteria0
                this.rule[0] = criteria0
                this.prevFieldName = newFN

                let newOperator = []
                if (_operator != null && _operator.length > 0) {
                    for (let op of _operator) {
                        newOperator.push(op)
                    }
                }
                this.prevOperator = newOperator
            }
        }
        //if (this.currentForm.form.get('criteria0')) {
        //    //this.currentForm.setValue(value)
        //}
        //if (this.currentForm) {
        //    this.currentForm.valueChanges
        //        .subscribe(data => this.onValueChanged(data));
        //}

        if (!this.currentForm) { return; }
        const form = this.currentForm.form;

        for (const field in form.controls) {
            // clear previous error message (if any)
            this.formErrors[field] = {};
            const control = form.get(field);

            if (control && !control.valid) {
                const messages = this.validationMessages['criteria0'];
                for (const subFieldName in control.errors) {
                    for (const errorKey in control.errors[subFieldName]) {
                        if (control.errors[subFieldName][errorKey]) {
                            let subfieldErrors = this.formErrors[field][subFieldName]
                            let subfieldError = messages[subFieldName][errorKey]
                            this.formErrors[field][subFieldName] = subfieldErrors != null ? subfieldErrors + subfieldError + ' ' : subfieldError + ' ';
                        }
                    }
                }
            }
        }

        this.cdRef.detectChanges();
    }

    //onValueChanged(data?: any) {
    //    if (!this.currentForm) { return; }
    //    const form = this.currentForm.form;

    //    for (const field in form.controls) {
    //        // clear previous error message (if any)
    //        this.formErrors[field] = {};
    //        const control = form.get(field);

    //        if (control && !control.valid) {
    //            const messages = this.validationMessages['criteria0'];
    //            for (const subFieldName in control.errors) {
    //                for (const errorKey in control.errors[subFieldName]) {
    //                    if (control.errors[subFieldName][errorKey] != null) {
    //                        let subfieldErrors = this.formErrors[field][subFieldName]
    //                        let subfieldError = messages[subFieldName][errorKey]
    //                        this.formErrors[field][subFieldName] = subfieldErrors != null ? subfieldErrors + subfieldError + ' ' : subfieldError + ' ';
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    this.cdRef.detectChanges();
    //}

    formErrors = {
        'criteria0': {}
    };

    validationMessages = {
        'criteria0': {
            fieldName: {
                'required': 'Field name is required.'
            },
            _operator: {
                'required': 'Field name is required.'
            }
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
        if (this.rule[index].fieldName === ['Province']) {
            this.provinces = null
        }
        this.rule.splice(index, 1)
    }
}