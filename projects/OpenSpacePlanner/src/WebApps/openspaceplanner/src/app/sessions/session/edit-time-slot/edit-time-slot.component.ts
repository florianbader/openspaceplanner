import { Component } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { TimeSlot } from '@rio-scaffolding/shared-backend-api';
import {
    InputTextAreaComponent,
    InputTextComponent,
    InputTimeComponent,
    OffCanvasComponent,
    OffCanvasComponentBase,
} from '@rio-scaffolding/shared-ui';
import { UpdateTimeSlot } from '../../sessions-state/actions/update-time-slot';

@Component({
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        OffCanvasComponent,
        InputTextComponent,
        InputTextAreaComponent,
        InputTimeComponent,
    ],
    selector: 'app-edit-time-slot',
    templateUrl: 'edit-time-slot.component.html',
})
export class EditTimeSlotComponent extends OffCanvasComponentBase<
    TimeSlot,
    UpdateTimeSlot
> {
    protected override initializeForm(formBuilder: FormBuilder): FormGroup {
        return formBuilder.group({
            name: ['', [Validators.required]],
            description: ['', []],
            fromTime: ['', []],
            toTime: ['', []],
        });
    }

    protected override createUpdateAction(entity: TimeSlot) {
        return new UpdateTimeSlot(entity);
    }
}
