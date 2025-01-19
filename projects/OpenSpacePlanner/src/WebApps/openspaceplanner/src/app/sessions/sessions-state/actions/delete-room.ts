import { StateContext, Store } from '@ngxs/store';
import { Room, RoomsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class DeleteRoom {
    public static readonly type = `${ACTION_SCOPE} Delete Room`;
    constructor(public id: string) {}
}

export const deleteRoomHandler =
    (roomsService: RoomsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: DeleteRoom) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return roomsService.deleteRoom(tenant, action.id, sessionId).pipe(
            tap(() => {
                return ctx.patchState({
                    rooms: ctx
                        .getState()
                        .rooms.filter((r: Room) => r.id !== action.id),
                });
            }),
        );
    };
