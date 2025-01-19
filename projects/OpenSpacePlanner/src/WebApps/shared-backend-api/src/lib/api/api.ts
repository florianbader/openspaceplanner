export * from './internationalization.service';
import { InternationalizationService } from './internationalization.service';
export * from './rooms.service';
import { RoomsService } from './rooms.service';
export * from './sessions.service';
import { SessionsService } from './sessions.service';
export * from './timeSlots.service';
import { TimeSlotsService } from './timeSlots.service';
export * from './topics.service';
import { TopicsService } from './topics.service';
export const APIS = [
    InternationalizationService,
    RoomsService,
    SessionsService,
    TimeSlotsService,
    TopicsService,
];
