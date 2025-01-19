import { StateContext, Store } from '@ngxs/store';
import { SessionsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';

const SESSIONS_PAGE_SIZE = 10;

export class GetSessions {
    public static readonly type = `${ACTION_SCOPE} Get Sessions`;
    constructor(public page: number = 1) {}
}

export const getSessionsHandler =
    (sessionsService: SessionsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: GetSessions) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());

        return sessionsService
            .getAllSessions(tenant, action.page, SESSIONS_PAGE_SIZE)
            .pipe(
                tap((sessions) => {
                    return ctx.patchState({
                        sessions: sessions.items,
                        pageResult: sessions.pages,
                    });
                }),
            );
    };
