import { StateContext, Store } from '@ngxs/store';
import { Session, SessionsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';

export class UpdateSession {
    public static readonly type = `${ACTION_SCOPE} Update Session`;
    constructor(public session: Session) {}
}

export const updateSessionHandler =
    (sessionsService: SessionsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: UpdateSession) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        return sessionsService
            .updateSession(tenant, action.session.id, action.session)
            .pipe(
                tap((session) => {
                    return ctx.patchState({
                        sessions: ctx
                            .getState()
                            .sessions.map((s: Session) =>
                                s.id === session.id ? session : s,
                            ),
                        currentSession: session,
                    });
                }),
            );
    };
