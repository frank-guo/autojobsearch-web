export class SearchCriteria {
    id: number;
    fieldName: string[];
    _operator: string[];
    values: string[];
    constructor();
    constructor(id: number, fieldName: string[], operator: string[], values: [any]);
    constructor(id?: number, fieldName?: string[], operator?: string[], values?: string[]) {
        this.id = id;
        this.fieldName = fieldName;
        this._operator = operator;
        this.values = values;
    }
}