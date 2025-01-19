import { StateContext, Store } from '@ngxs/store';
import { Topic, TopicsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class UpdateTopic {
    public static readonly type = `${ACTION_SCOPE} Update Topic`;
    constructor(public topic: Topic) {}
}

export const updateTopicHandler =
    (topicsService: TopicsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: UpdateTopic) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return topicsService
            .updateTopic(tenant, sessionId, action.topic.id, action.topic)
            .pipe(
                tap((topic) => {
                    return ctx.patchState({
                        topics: ctx
                            .getState()
                            .topics.map((r: Topic) =>
                                r.id === topic.id ? topic : r,
                            ),
                    });
                }),
            );
    };
