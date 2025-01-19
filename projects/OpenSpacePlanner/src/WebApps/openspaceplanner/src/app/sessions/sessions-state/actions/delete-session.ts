import { StateContext, Store } from '@ngxs/store';
import { Session, SessionsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';

export class DeleteSession {
    public static readonly type = `${ACTION_SCOPE} Delete Session`;
    constructor(public id: string) {}
}

export const deleteSessionHandler =
    (sessionsService: SessionsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: DeleteSession) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        return sessionsService.deleteSession(tenant, action.id).pipe(
            tap(() => {
                return ctx.patchState({
                    sessions: ctx
                        .getState()
                        .sessions.filter((s: Session) => s.id !== action.id),
                    currentSession: null,
                });
            }),
        );
    };
