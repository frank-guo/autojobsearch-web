import { NG_VALIDATORS, Validator, AbstractControl, ValidatorFn, FormControl } from '@angular/forms';
import { Directive } from '@angular/core';

function validateSearchCriteriaFactory(): ValidatorFn {
    return (c: AbstractControl) => {

        let isValid = c.value === 'Juri';

        if (isValid) {
            return null;
        } else {
            return {
                requiredAny: {
                    valid: false
                }
            };
        }
    }
}

//Tommorrow solve the issue that this fucntion doesn't get called when fieldName value changes
function validateSearchCriteria(c: FormControl) {
    if (c.value == null) {
        return null
    }
    let fieldName = c.value.fieldName;
    let fieldNameEmpty = fieldName == null || fieldName.length === 0;
    //let length = fieldName == null ? 0 : fieldName.length
    let _operator = c.value._operator
    let operatorEmpty = _operator == null || _operator.length === 0
    let values = c.value.values  
    let valuesEmpty = values == null || values.length === 0

    if (!fieldNameEmpty && !operatorEmpty && !valuesEmpty) {
        return null;
    } else {
        return {
            fieldName: {
                required: fieldNameEmpty ? true : false,
               // minlength: length < 10 && !fieldNameEmpty ? 'minlength' : null
            },
            _operator: {
                required: operatorEmpty ? true : false
            },
            values: {
                required: valuesEmpty ? true : false
            }
        };
    }
}


@Directive({
    selector: '[validateSearchCriteria][ngModel]',
    providers: [
        { provide: NG_VALIDATORS, useValue: validateSearchCriteria, multi: true }
    ]
})
export class SearchCriteriaValidator {

}