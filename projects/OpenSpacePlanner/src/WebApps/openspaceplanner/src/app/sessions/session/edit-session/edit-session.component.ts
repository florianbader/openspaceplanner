import { Component } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators,
} from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { Session } from '@rio-scaffolding/shared-backend-api';
import {
    InputTextComponent,
    OffCanvasComponent,
    OffCanvasComponentBase,
} from '@rio-scaffolding/shared-ui';
import { UpdateSession } from '../../sessions-state/actions/update-session';

@Component({
    standalone: true,
    imports: [
        TranslateModule,
        ReactiveFormsModule,
        OffCanvasComponent,
        InputTextComponent,
    ],
    selector: 'app-edit-session',
    templateUrl: 'edit-session.component.html',
})
export class EditSessionComponent extends OffCanvasComponentBase<
    Session,
    UpdateSession
> {
    protected override initializeForm(formBuilder: FormBuilder): FormGroup {
        return formBuilder.group({
            name: ['', [Validators.required]],
        });
    }

    protected override createUpdateAction(entity: Session) {
        return new UpdateSession(entity);
    }
}
