import { Component, input } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    standalone: true,
    imports: [TranslateModule, ReactiveFormsModule],
    selector: 'shared-input-number',
    template: ` <div class="mb-3">
        <label for="{{ id() }}" class="form-label">{{
            titleTranslateKey() | translate
        }}</label>
        <input
            type="number"
            id="{{ id() }}"
            name="{{ id() }}"
            class="form-control"
            formControlName="{{ id() }}"
        />
    </div>`,
})
export class InputNumberComponent {
    public id = input('id');
    public titleTranslateKey = input('title');
}
