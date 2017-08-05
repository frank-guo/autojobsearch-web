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

function validateSearchCriteria(c: FormControl) {
    let fieldName = c.value ? c.value.fieldName : null;
    let fieldNameEmpty = fieldName == null || fieldName === '';
    let length = fieldName == null ? 0 : fieldName.length

    if (!fieldNameEmpty && length >= 10) {
        return null;
    } else {
        return {
            required: fieldNameEmpty ? 'required' : null,
            minlength: length < 10 && !fieldNameEmpty ? 'minlength' : null 
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