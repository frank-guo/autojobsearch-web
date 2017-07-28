import { Component, Input, SimpleChanges } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { cities, provinces, titles, allCities } from '../constant/droptown-options';
@Component({
    selector: 'search-criteria',
    templateUrl: './search-criteria.component.html',
    styleUrls: ['./ng2-select.css']
})
export class SearchCriteriaComponent {
    public fields: Array<string> = ['Province', 'City', 'Title', 'Time', 'Company Name', 'Responsibilities', 'Experience'];
    public operators: Array<string> = ['equal to', 'not equal to', 'starts with', 'contains', 'does not contain', 'less than', 'greater than', 'within'];
    private valuesObj = {
        Province: provinces,
        City: cities,
        Title: titles
    }
    public valuesOptions: string[]
    active: Array<string> = [this.fields[3]]

    @Input() model: SearchCriteria;
    @Input() onDeleteClick: Function;
    @Input() index: number;
    @Input() provinces: string[]
    @Input() formErrors: {}

    submitted = false;
    onSubmit() { this.submitted = true; }
    private fieldName: any;

    ngOnInit() {
        if (this.model == null) {
            return
        }
        this.fieldName = this.model.fieldName != null ? [{ id:this.model.fieldName, text: this.model.fieldName }] : null
        if (this.model.fieldName === 'City') {
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
            this.valuesOptions = this.valuesObj[this.model.fieldName]
        }
    }

    ngOnChanges(changes: SimpleChanges) {
        if (this.model == null) {
            return
        }
        if (changes['provinces'] != null && this.model.fieldName === 'City') {
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
        if (changes['model'] != null) {
            this.fieldName = this.model.fieldName
        }
    }

    ngModelOnChange(value: any) {
        this.model.fieldName = value
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
        if (this.model.fieldName === 'City') {
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
            this.valuesOptions = this.model.fieldName ? this.valuesObj[this.model.fieldName] : null;
        }
    }

    //ngAfterViewInit () {
    //    this.intialField = this.model.fieldName;
    //    this.intialOperator = this.model.operator;
    //    this.intialCities = this.model.values;
    //}

    public refreshField(value: any): void {
        this.model.fieldName = value ? value.id : null;
        if (this.model.fieldName === 'City') {
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
            this.valuesOptions = this.model.fieldName ? this.valuesObj[this.model.fieldName] : null;
        }
    }

    public refreshOperator(value: any): void {
        this.model._operator = value ? value.id : null;
        console.log('this.model.operator=', this.model._operator)
    }

    public refreshCity(values: [any]): void {
        let vals: string[] = []
        if (values != null) {
            values.map((value) => {
                vals.push(value.id)
            })
        } else {
            vals = null
        }
        this.model.values = vals;
    }

    public selected(value: any): void {
        console.log('Selected value is: ', value);
    }

    public removed(value: any): void {
        console.log('Removed value is: ', value);
    }

    public valuesTyped(value: any): void {
        let vals: string[] = []
        if (value != null) {
            vals.push(value)
        }

        this.model.values = vals;
    }
}