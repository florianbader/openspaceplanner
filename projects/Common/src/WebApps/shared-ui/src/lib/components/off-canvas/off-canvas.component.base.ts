import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActionType } from '@ngxs/store';
import { ComponentBase, OffCanvasComponent } from '@rio-scaffolding/shared-ui';
import { identity } from 'rxjs';

@Component({
    template: '',
})
export abstract class OffCanvasComponentBase<TEntity, TSaveAction>
    extends ComponentBase
    implements OnInit
{
    private readonly _formBuilder = inject(FormBuilder);
    private _entity?: TEntity;

    @ViewChild(OffCanvasComponent)
    private readonly _offCanvas!: OffCanvasComponent;

    public formGroup: FormGroup;

    constructor() {
        super();

        this.formGroup = this.initializeForm(this._formBuilder);
    }

    public ngOnInit() {
        this.addLoader([
            identity<TSaveAction>({} as TSaveAction) as ActionType,
        ]);
    }

    public open(entity: TEntity) {
        this._entity = entity;

        this.formGroup.patchValue(entity as Partial<{}>);

        this._offCanvas.open();
    }

    public save() {
        const entity = {
            ...this._entity,
            ...this.formGroup.value,
        } as TEntity;

        if (this.formGroup.invalid) {
            return;
        }

        const action = this.createUpdateAction(entity);
        this.store.dispatch(action);
    }

    protected abstract createUpdateAction(entity: TEntity): void;

    protected abstract initializeForm(formBuilder: FormBuilder): FormGroup;
}
