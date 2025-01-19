import { StateContext, Store } from '@ngxs/store';
import { Session, SessionsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';

export class GetSession {
    public static readonly type = `${ACTION_SCOPE} Get Session`;
    constructor(public id: string) {}
}

export const getSessionHandler =
    (sessionsService: SessionsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: GetSession) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());

        return sessionsService.getSession(tenant, action.id).pipe(
            tap((session: Session) => {
                return ctx.patchState({
                    currentSession: session,
                    rooms: session.rooms,
                    timeSlots: session.timeSlots,
                    topics: session.topics,
                });
            }),
        );
    };
