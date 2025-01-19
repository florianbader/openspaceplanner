import { createSelector } from '@ngxs/store';
import { environment } from '../../environments/environment';
import { AppState } from './app.state';
import { AppStateModel } from './app.state.model';

export class AppSelectors {
    public static tenant() {
        return createSelector(
            [AppState],
            (state: AppStateModel) => state.tenant ?? environment.defaultTenant,
        );
    }

    public static breadcrumbs() {
        return createSelector(
            [AppState],
            (state: AppStateModel) => state.breadcrumbs,
        );
    }
}
