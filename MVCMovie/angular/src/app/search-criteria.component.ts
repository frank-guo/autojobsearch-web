import { Component, Input } from '@angular/core';
import { SearchCriteria } from './search-criteria';
@Component({
    selector: 'search-criteria',
    templateUrl: './search-criteria.component.html',
    styleUrls: ['./ng2-select.css']
})
export class SearchCriteriaComponent {
    public fields: Array<string> = ['Province', 'City', 'Title', 'Time', 'Company Name', 'Responsibilities', 'Experience'];
    public operators: Array<string> = ['equal', 'not equal to', 'starts with', 'contains', 'does not contain', 'less than', 'greater than', 'within'];
    public cities: Array<string> = ['Vancouver', 'Burnaby', 'Richmond', 'Coquitlam', 'Surrey', 'Port Coquitlam'];
    public provinces: Array<string> = ['BC', 'ON', 'AB'];
    @Input() model: SearchCriteria;
    submitted = false;
    onSubmit() { this.submitted = true; }

    //ngAfterViewInit () {
    //    this.intialField = this.model.fieldName;
    //    this.intialOperator = this.model.operator;
    //    this.intialCities = this.model.values;
    //}

    public refreshField(value: any): void {
        this.model.fieldName = value ? value.id : null;
        console.log('this.model.fieldName', this.model.fieldName)
    }

    public refreshOperator(value: any): void {
        this.model.operator = value ? value.id : null;
        console.log('this.model.operator=', this.model.operator)
    }

    public refreshCity(values: [any]): void {
        this.model.values = values ? values : null;
    }

    public selected(value: any): void {
        console.log('Selected value is: ', value);
    }

    public removed(value: any): void {
        console.log('Removed value is: ', value);
    }
}