import { StateContext, Store } from '@ngxs/store';
import {
    CreateTopicCommand,
    TopicsService,
} from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class CreateTopic {
    public static readonly type = `${ACTION_SCOPE} Create Topic`;
    constructor(public topic: CreateTopicCommand) {}
}

export const createTopicHandler =
    (topicsService: TopicsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: CreateTopic) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return topicsService.createTopic(tenant, sessionId, action.topic).pipe(
            tap((topic) => {
                return ctx.patchState({
                    topics: [topic, ...ctx.getState().topics],
                });
            }),
        );
    };
