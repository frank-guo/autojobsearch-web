import { Directive, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { SelectComponent } from 'ng2-select';

export const CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR: any = {
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => NgModalSelect),
    multi: true
};

@Directive({
    selector: 'ngmdoel-select',
    host: { '(selected)': 'onChange($event)' },
    providers: [CUSTOM_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class NgModalSelect implements ControlValueAccessor {
    constructor(private host: SelectComponent) {

    }

    onChange = (_) => { };
    onTouched = () => { };

    //From ControlValueAccessor interface
    writeValue(value: any) {
        this.host.active = value
    }

    registerOnChange(fn: (_: any) => void): void { this.onChange = fn; }
    registerOnTouched(fn: () => void): void { this.onTouched = fn; }
}