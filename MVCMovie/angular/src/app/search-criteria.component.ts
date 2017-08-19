import { Component, Input, SimpleChanges, forwardRef } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { cities, provinces, titles, allCities } from '../constant/droptown-options';
import {NG_VALUE_ACCESSOR, ControlValueAccessor, NG_VALIDATORS, FormControl, Validator } from '@angular/forms';

const noop = () => {
};

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => SearchCriteriaComponent),
    multi: true
}

@Component({
    selector: 'search-criteria',
    templateUrl: './search-criteria.component.html',
    styleUrls: ['./ng2-select.css'],
    providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class SearchCriteriaComponent implements ControlValueAccessor{
    public fields: Array<string> = ['Province', 'City', 'Title', 'Time', 'Company Name', 'Responsibilities', 'Experience'];
    public operators: Array<string> = ['equal to', 'not equal to', 'starts with', 'contains', 'does not contain', 'less than', 'greater than', 'within'];
    private valuesObj = {
        Province: provinces,
        City: cities,
        Title: titles
    }
    public valuesOptions: string[]
    active: Array<string> = [this.fields[3]]

    private model: any;
    @Input() onDeleteClick: Function;
    @Input() index: number;
    @Input() provinces: string[]
    @Input() errors: any

    submitted = false;
    onSubmit() { this.submitted = true; }

    ngOnInit() {
        if (this.model == null) {
            return
        }

        if (this.model.fieldName === ['City']) {
            if (this.provinces != null && this.provinces.length > 0) {
                this.valuesOptions = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.valuesOptions = this.valuesOptions.concat(_cities)
                })
            } else {
                this.valuesOptions = allCities()
            }
        } else {
            this.valuesOptions = this.valuesObj[this.model.fieldName[0]]
        }
    }

    ngOnChanges(changes: SimpleChanges) {
        if (this.model == null) {
            return
        }
        if (changes['provinces'] != null && this.model.fieldName === ['City']) {
            if (this.provinces != null && this.provinces.length > 0) {
                this.valuesOptions = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.valuesOptions = this.valuesOptions.concat(_cities)
                })
            } else {
                this.valuesOptions = allCities()
            }
        }
    }

    ngAfterContentChecked() {
        if (this.model == null) {
            return
        }

        let fieldName = this.model.fieldName
        if (fieldName != null && (fieldName[0] === 'City' || fieldName[0] != null && fieldName[0].id === 'City')) {
            if (this.provinces != null && this.provinces.length > 0) {
                this.valuesOptions = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.valuesOptions = this.valuesOptions.concat(_cities)
                })
            } else {
                this.valuesOptions = allCities()
            }
        } else {
            let fieldName0 = fieldName ? fieldName[0] : null
            this.valuesOptions = fieldName && fieldName0 ? this.valuesObj[fieldName0.id ? fieldName0.id : fieldName0] : null
        }
    }

    //onNgModelValuesChange(event) {
    //    let newValues = []
    //    for (let value of event) {
    //        newValues.push(value.id)
    //    }
    //    this.model.values = newValues
    //}

    //Placeholders for the callbacks which are later provided
    //by the Control Value Accessor
    private onTouchedCallback: () => void = noop;
    private onChangeCallback: (_: any) => void = noop;

    //get accessor
    get value(): any {
        return this.model;
    };

    //set accessor including call the onchange callback
    set value(v: any) {
        if (v !== this.model) {
            this.model = v;
            this.onChangeCallback(v);
        }
    }

    //Set touched on blur
    onBlur() {
        this.onTouchedCallback();
    }

    //From ControlValueAccessor interface
    writeValue(value: any) {
        if (value !== this.model) {
            this.model = value;
        }
    }

    //From ControlValueAccessor interface
    registerOnChange(fn: any) {
        this.onChangeCallback = fn;
    }

    //From ControlValueAccessor interface
    registerOnTouched(fn: any) {
        this.onTouchedCallback = fn;
    }

    public setValues(filedNameValues: any): void {
        if (!filedNameValues || filedNameValues && filedNameValues.length === 0) {
            this.model.values = []
            return
        }
        if (filedNameValues && filedNameValues[0].id === this.model.fieldName) {
            return
        }
        this.model.values = []
        this.model.fieldName = filedNameValues ? filedNameValues[0].id : null;
        if (this.model.fieldName === ['City']) {
            if (this.provinces != null && this.provinces.length > 0) {
                this.valuesOptions = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.valuesOptions = this.valuesOptions.concat(_cities)
                })
            } else {
                this.valuesOptions = allCities()
            }
        } else {
            this.valuesOptions = this.model.fieldName ? this.valuesObj[this.model.fieldName[0]] : null;
        }
    }

    //The following block of code is no longer useful if using ngModel
    //ngAfterViewInit () {
    //    this.intialField = this.model.fieldName;
    //    this.intialOperator = this.model.operator;
    //    this.intialCities = this.model.values;
    //}

    //public refreshField(value: any): void {
    //    this.model.fieldName = value ? value.id : null;
    //    if (this.model.fieldName === ['City']) {
    //        if (this.provinces != null && this.provinces.length > 0) {
    //            this.valuesOptions = []
    //            this.provinces.map((province) => {
    //                let _cities = this.valuesObj.City[province]
    //                this.valuesOptions = this.valuesOptions.concat(_cities)
    //            })
    //        } else {
    //            this.valuesOptions = allCities()
    //        }
    //    } else {
    //        this.valuesOptions = this.model.fieldName ? this.valuesObj[this.model.fieldName[0]] : null;
    //    }
    //}

    //public refreshOperator(value: any): void {
    //    this.model._operator = value ? value.id : null;
    //    console.log('this.model.operator=', this.model._operator)
    //}

    //public refreshCity(values: [any]): void {
    //    let vals: string[] = []
    //    if (values != null) {
    //        values.map((value) => {
    //            vals.push(value.id)
    //        })
    //    } else {
    //        vals = null
    //    }
    //    this.model.values = vals;
    //}

    //public selected(value: any): void {
    //    console.log('Selected value is: ', value);
    //}

    //public removed(value: any): void {
    //    console.log('Removed value is: ', value);
    //}

    //public valuesTyped(value: any): void {
    //    let vals: string[] = []
    //    if (value != null) {
    //        vals.push(value)
    //    }

    //    this.model.values = vals;
    //}


    /*
    public validate(c: FormControl) {
        let ret =  c.value && c.value.fieldName !== '' ? null : {
            fieldName: {
                valid: true,
            },
        };
        return null;
    }
    */
}