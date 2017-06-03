export class SearchCriteria {
    fieldName: string;
    operator: string;
    values: [string];
    constructor();
    constructor(fieldName: string, operator: string, values: [any]);
    constructor(fieldName?: string, operator?: string, values?: [any]) {
        this.fieldName = fieldName;
        this.operator = operator;
        this.values = values;
    }
}