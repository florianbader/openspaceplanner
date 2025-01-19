import { StateContext, Store } from '@ngxs/store';
import { Topic, TopicsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class DeleteTopic {
    public static readonly type = `${ACTION_SCOPE} Delete Topic`;
    constructor(public id: string) {}
}

export const deleteTopicHandler =
    (topicsService: TopicsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: DeleteTopic) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return topicsService.deleteTopic(tenant, action.id, sessionId).pipe(
            tap(() => {
                return ctx.patchState({
                    topics: ctx
                        .getState()
                        .topics.filter((r: Topic) => r.id !== action.id),
                });
            }),
        );
    };
