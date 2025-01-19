import { Injectable } from '@angular/core';
import { attachAction } from '@ngxs-labs/attach-action';
import { State, StateToken, Store } from '@ngxs/store';
import {
    RoomsService,
    SessionsService,
    TimeSlotsService,
    TopicsService,
} from '@rio-scaffolding/shared-backend-api';
import { CreateRoom, createRoomHandler } from './actions/create-room';
import { CreateSession, createSessionHandler } from './actions/create-session';
import {
    CreateTimeSlot,
    createTimeSlotHandler,
} from './actions/create-time-slot';
import { CreateTopic, createTopicHandler } from './actions/create-topic';
import { DeleteRoom, deleteRoomHandler } from './actions/delete-room';
import { DeleteSession, deleteSessionHandler } from './actions/delete-session';
import {
    DeleteTimeSlot,
    deleteTimeSlotHandler,
} from './actions/delete-time-slot';
import { DeleteTopic, deleteTopicHandler } from './actions/delete-topic';
import { GetSession, getSessionHandler } from './actions/get-session';
import { GetSessions, getSessionsHandler } from './actions/get-sessions';
import { UpdateRoom, updateRoomHandler } from './actions/update-room';
import { UpdateSession, updateSessionHandler } from './actions/update-session';
import {
    UpdateTimeSlot,
    updateTimeSlotHandler,
} from './actions/update-time-slot';
import { UpdateTopic, updateTopicHandler } from './actions/update-topic';
import { stateDefaultValues } from './sessions.state.defaults';
import { SessionsStateModel } from './sessions.state.model';

export const ACTION_SCOPE = '[Sessions]';

const STATE_TOKEN = new StateToken<SessionsStateModel[]>('Sessions');

@State<SessionsStateModel>({
    name: STATE_TOKEN,
    defaults: stateDefaultValues,
})
@Injectable()
export class SessionsState {
    constructor(
        store: Store,
        sessionsService: SessionsService,
        roomsService: RoomsService,
        timeSlotsService: TimeSlotsService,
        topicService: TopicsService,
    ) {
        attachAction(
            SessionsState,
            GetSessions,
            getSessionsHandler(sessionsService, store),
        );
        attachAction(
            SessionsState,
            CreateSession,
            createSessionHandler(sessionsService, store),
        );
        attachAction(
            SessionsState,
            GetSession,
            getSessionHandler(sessionsService, store),
        );
        attachAction(
            SessionsState,
            UpdateSession,
            updateSessionHandler(sessionsService, store),
        );
        attachAction(
            SessionsState,
            DeleteSession,
            deleteSessionHandler(sessionsService, store),
        );

        attachAction(
            SessionsState,
            CreateRoom,
            createRoomHandler(roomsService, store),
        );
        attachAction(
            SessionsState,
            UpdateRoom,
            updateRoomHandler(roomsService, store),
        );
        attachAction(
            SessionsState,
            DeleteRoom,
            deleteRoomHandler(roomsService, store),
        );

        attachAction(
            SessionsState,
            CreateTimeSlot,
            createTimeSlotHandler(timeSlotsService, store),
        );
        attachAction(
            SessionsState,
            UpdateTimeSlot,
            updateTimeSlotHandler(timeSlotsService, store),
        );
        attachAction(
            SessionsState,
            DeleteTimeSlot,
            deleteTimeSlotHandler(timeSlotsService, store),
        );

        attachAction(
            SessionsState,
            CreateTopic,
            createTopicHandler(topicService, store),
        );
        attachAction(
            SessionsState,
            UpdateTopic,
            updateTopicHandler(topicService, store),
        );
        attachAction(
            SessionsState,
            DeleteTopic,
            deleteTopicHandler(topicService, store),
        );
    }
}
