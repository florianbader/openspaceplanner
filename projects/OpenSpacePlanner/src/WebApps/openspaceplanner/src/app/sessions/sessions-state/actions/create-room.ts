import { StateContext, Store } from '@ngxs/store';
import {
    CreateRoomCommand,
    RoomsService,
} from '@rio-scaffolding/shared-backend-api';
import { tap } from 'rxjs';
import { AppSelectors } from '../../../app-state/app.state.selectors';
import { ACTION_SCOPE } from '../sessions.state';
import { SessionsStateModel } from '../sessions.state.model';
import { SessionsSelectors } from '../sessions.state.selectors';

export class CreateRoom {
    public static readonly type = `${ACTION_SCOPE} Create Room`;
    constructor(public room: CreateRoomCommand) {}
}

export const createRoomHandler =
    (roomsService: RoomsService, store: Store) =>
    (ctx: StateContext<SessionsStateModel>, action: CreateRoom) => {
        const tenant = store.selectSnapshot(AppSelectors.tenant());
        const sessionId = store.selectSnapshot(
            SessionsSelectors.currentSession(),
        )?.id;
        if (sessionId == null) {
            return;
        }

        return roomsService.createRoom(tenant, sessionId, action.room).pipe(
            tap((room) => {
                return ctx.patchState({
                    rooms: [room, ...ctx.getState().rooms],
                });
            }),
        );
    };
