import {
    PageResult,
    Room,
    Session,
    TimeSlot,
    Topic,
} from '@rio-scaffolding/shared-backend-api';

export interface SessionsStateModel {
    sessions: Session[];
    pageResult: PageResult | null;
    currentSession: Session | null;
    rooms: Room[];
    timeSlots: TimeSlot[];
    topics: Topic[];
}
