import { StateContext } from '@ngxs/store';
import { UpdateBreadcrumbs } from '@rio-scaffolding/shared-ui';
import { AppStateModel } from '../app.state.model';

export const updateBreadcrumbsHandler =
    () => (ctx: StateContext<AppStateModel>, action: UpdateBreadcrumbs) => {
        return ctx.patchState({
            breadcrumbs: action.breadcrumbs,
        });
    };
