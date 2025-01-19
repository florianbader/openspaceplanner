import { StateContext, Store } from '@ngxs/store';
import {
    TimeSlot,
    TimeSlotsService,
} from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class DeleteTimeSlot {
    public static readonly type = `${ACTION_SCOPE} Delete TimeSlot`;
    constructor(public id: string) {}
}

export const deleteTimeSlotHandler =
    (timeSlotsService: TimeSlotsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: DeleteTimeSlot) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return timeSlotsService
            .deleteTimeSlot(tenant, action.id, sessionId)
            .pipe(
                tap(() => {
                    return ctx.patchState({
                        timeSlots: ctx
                            .getState()
                            .timeSlots.filter(
                                (r: TimeSlot) => r.id !== action.id,
                            ),
                    });
                }),
            );
    };
