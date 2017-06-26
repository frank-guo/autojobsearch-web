import { Component, Input } from '@angular/core';
import { SearchCriteria } from './search-criteria';
import { cities, provinces, titles } from '../constant/droptown-options';
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
    public values: string[]

    @Input() model: SearchCriteria;
    @Input() onDeleteClick: Function;
    @Input() index: number;
    @Input() provinces: string[]
    submitted = false;
    onSubmit() { this.submitted = true; }

    ngOnInit() {
        if (this.model.fieldName === 'City') {
            if (this.provinces != null) {
                this.values = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.values = this.values.concat(_cities)
                })
            }
        } else {
            this.values = this.valuesObj[this.model.fieldName]
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
            if (this.provinces != null) {
                this.values = []
                this.provinces.map((province) => {
                    let _cities = this.valuesObj.City[province]
                    this.values = this.values.concat(_cities)
                })
            }
        } else {
            this.values = this.model.fieldName ? this.valuesObj[this.model.fieldName] : null;
        }
        console.log('this.model.fieldName', this.model.fieldName)
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