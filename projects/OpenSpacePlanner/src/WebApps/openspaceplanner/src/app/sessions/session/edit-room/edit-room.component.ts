import { Component } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Room } from '@rio-scaffolding/shared-backend-api';
import {
    InputTextAreaComponent,
    InputTextComponent,
    OffCanvasComponent,
    OffCanvasComponentBase,
} from '@rio-scaffolding/shared-ui';
import { UpdateRoom } from '../../sessions-state/actions/update-room';

@Component({
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        OffCanvasComponent,
        InputTextComponent,
        InputTextAreaComponent,
    ],
    selector: 'app-edit-room',
    templateUrl: 'edit-room.component.html',
})
export class EditRoomComponent extends OffCanvasComponentBase<
    Room,
    UpdateRoom
> {
    protected override initializeForm(formBuilder: FormBuilder): FormGroup {
        return formBuilder.group({
            name: ['', [Validators.required]],
            description: ['', []],
        });
    }

    protected override createUpdateAction(entity: Room) {
        return new UpdateRoom(entity);
    }
}
