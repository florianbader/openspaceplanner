import { createSelector } from '@ngxs/store';
import { SessionsState } from './sessions.state';
import { SessionsStateModel } from './sessions.state.model';

export class SessionsSelectors {
    public static sessions() {
        return createSelector(
            [SessionsState],
            (state: SessionsStateModel) => state.sessions,
        );
    }

    public static pageResult() {
        return createSelector(
            [SessionsState],
            (state: SessionsStateModel) => state.pageResult,
        );
    }

    public static currentSession() {
        return createSelector(
            [SessionsState],
            (state: SessionsStateModel) => state.currentSession,
        );
    }

    public static rooms() {
        return createSelector(
            [SessionsState],
            (state: SessionsStateModel) => state.rooms,
        );
    }

    public static timeSlots() {
        return createSelector(
            [SessionsState],
            (state: SessionsStateModel) => state.timeSlots,
        );
    }

    public static assignedTopics() {
        return createSelector([SessionsState], (state: SessionsStateModel) =>
            state.topics.filter(
                (t) => t.roomId != null && t.timeSlotId != null,
            ),
        );
    }

    public static unassignedTopics() {
        return createSelector([SessionsState], (state: SessionsStateModel) =>
            state.topics.filter(
                (t) => t.roomId == null && t.timeSlotId == null,
            ),
        );
    }
}
