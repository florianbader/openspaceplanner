import { StateContext, Store } from '@ngxs/store';
import { Room, RoomsService } from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class UpdateRoom {
    public static readonly type = `${ACTION_SCOPE} Update Room`;
    constructor(public room: Room) {}
}

export const updateRoomHandler =
    (roomsService: RoomsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: UpdateRoom) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return roomsService
            .updateRoom(tenant, action.room.id, sessionId, action.room)
            .pipe(
                tap((room) => {
                    return ctx.patchState({
                        rooms: ctx
                            .getState()
                            .rooms.map((r: Room) =>
                                r.id === room.id ? room : r,
                            ),
                    });
                }),
            );
    };
