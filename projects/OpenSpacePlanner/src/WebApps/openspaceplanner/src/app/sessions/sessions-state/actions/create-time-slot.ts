import { StateContext, Store } from '@ngxs/store';
import {
    CreateTimeSlotCommand,
    TimeSlotsService,
} from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class CreateTimeSlot {
    public static readonly type = `${ACTION_SCOPE} Create TimeSlot`;
    constructor(public timeSlot: CreateTimeSlotCommand) {}
}

export const createTimeSlotHandler =
    (timeSlotsService: TimeSlotsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: CreateTimeSlot) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return timeSlotsService
            .createTimeSlot(tenant, sessionId, action.timeSlot)
            .pipe(
                tap((timeSlot) => {
                    return ctx.patchState({
                        timeSlots: [timeSlot, ...ctx.getState().timeSlots],
                    });
                }),
            );
    };
