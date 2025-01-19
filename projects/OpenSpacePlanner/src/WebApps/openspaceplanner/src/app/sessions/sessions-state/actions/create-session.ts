import { StateContext, Store } from '@ngxs/store';
import {
    CreateSessionCommand,
    SessionsService,
} from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';

export class CreateSession {
    public static readonly type = `${ACTION_SCOPE} Create Session`;
    constructor(public session: CreateSessionCommand) {}
}

export const createSessionHandler =
    (sessionsService: SessionsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: CreateSession) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        return sessionsService.createSession(tenant, action.session).pipe(
            tap((session) => {
                return ctx.patchState({
                    sessions: [session, ...ctx.getState().sessions],
                    currentSession: session,
                });
            }),
        );
    };
