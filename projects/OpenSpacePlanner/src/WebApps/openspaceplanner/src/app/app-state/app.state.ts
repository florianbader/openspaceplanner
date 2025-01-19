import { Injectable } from '@angular/core';
import { attachAction } from '@ngxs-labs/attach-action';
import { State, StateToken } from '@ngxs/store';
import { UpdateBreadcrumbs } from '@rio-scaffolding/shared-ui';
import { updateBreadcrumbsHandler } from './actions/update-breadcrumbs';
import { appStateDefaultValues } from './app.state.defaults';
import { AppStateModel } from './app.state.model';

const APP_STATE_TOKEN = new StateToken<AppStateModel[]>('App');

@State<AppStateModel>({
    name: APP_STATE_TOKEN,
    defaults: appStateDefaultValues,
})
@Injectable()
export class AppState {
    constructor() {
        attachAction(AppState, UpdateBreadcrumbs, updateBreadcrumbsHandler());
    }
}
