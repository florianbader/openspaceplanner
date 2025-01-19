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

export class UpdateTimeSlot {
    public static readonly type = `${ACTION_SCOPE} Update TimeSlot`;
    constructor(public timeSlot: TimeSlot) {}
}

export const updateTimeSlotHandler =
    (timeSlotsService: TimeSlotsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: UpdateTimeSlot) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return timeSlotsService
            .updateTimeSlot(
                tenant,
                action.timeSlot.id,
                sessionId,
                action.timeSlot,
            )
            .pipe(
                tap((timeSlot) => {
                    return ctx.patchState({
                        timeSlots: ctx
                            .getState()
                            .timeSlots.map((r: TimeSlot) =>
                                r.id === timeSlot.id ? timeSlot : r,
                            ),
                    });
                }),
            );
    };
