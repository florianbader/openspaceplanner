import { Component, Input, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TranslateModule } from '@ngx-translate/core';
import { select } from '@ngxs/store';
import { Topic } from '@rio-scaffolding/shared-backend-api';
import {
    ComponentBase,
    FloatingActionButtonComponent,
    FloatingActionButtonItemComponent,
    HoverButtonContainerDirective,
    HoverEditButtonComponent,
} from '@rio-scaffolding/shared-ui';
import { takeUntil } from 'rxjs';
import { CreateRoom } from '../sessions-state/actions/create-room';
import { CreateTimeSlot } from '../sessions-state/actions/create-time-slot';
import { CreateTopic } from '../sessions-state/actions/create-topic';
import { DeleteRoom } from '../sessions-state/actions/delete-room';
import { DeleteTimeSlot } from '../sessions-state/actions/delete-time-slot';
import { DeleteTopic } from '../sessions-state/actions/delete-topic';
import { GetSession } from '../sessions-state/actions/get-session';
import { UpdateRoom } from '../sessions-state/actions/update-room';
import { UpdateTimeSlot } from '../sessions-state/actions/update-time-slot';
import { UpdateTopic } from '../sessions-state/actions/update-topic';
import { SessionsSelectors } from '../sessions-state/sessions.state.selectors';
import { TopicItemComponent } from '../topic-item/topic-item.component';
import { EditRoomComponent } from './edit-room/edit-room.component';
import { EditSessionComponent } from './edit-session/edit-session.component';
import { EditTimeSlotComponent } from './edit-time-slot/edit-time-slot.component';

@Component({
    standalone: true,
    imports: [
        FontAwesomeModule,
        TranslateModule,
        FloatingActionButtonComponent,
        FloatingActionButtonItemComponent,
        TopicItemComponent,
        HoverButtonContainerDirective,
        HoverEditButtonComponent,
        EditRoomComponent,
        EditSessionComponent,
        EditTimeSlotComponent,
    ],
    selector: 'app-session',
    templateUrl: 'session.component.html',
    styleUrl: 'session.component.scss',
})
export class SessionComponent extends ComponentBase implements OnInit {
    public session = select(SessionsSelectors.currentSession());
    public rooms = select(SessionsSelectors.rooms());
    public timeSlots = select(SessionsSelectors.timeSlots());
    public assignedTopics = select(SessionsSelectors.assignedTopics());
    public unassignedTopics = select(SessionsSelectors.unassignedTopics());

    @Input()
    public set sessionId(value: string) {
        this.store
            .dispatch(new GetSession(value))
            .pipe(takeUntil(this.destroy$))
            .subscribe(() => this.sessionLoaded());
    }

    public ngOnInit() {
        this.addLoader([
            GetSession,
            CreateRoom,
            UpdateRoom,
            DeleteRoom,
            CreateTimeSlot,
            UpdateTimeSlot,
            DeleteTimeSlot,
            CreateTopic,
            UpdateTopic,
            DeleteTopic,
        ]);
    }

    public getTopic(timeSlotId: string, roomId: string): Topic | null {
        const filteredTopics = this.assignedTopics().filter(
            (t) => t.timeSlotId === timeSlotId && t.roomId === roomId,
        );
        return filteredTopics.length > 0 ? filteredTopics[0] : null;
    }

    public createRoom() {
        this.store.dispatch(
            new CreateRoom({ name: this.translate.instant('room') }),
        );
    }

    public createTimeSlot() {
        this.store.dispatch(
            new CreateTimeSlot({ name: this.translate.instant('timeSlot') }),
        );
    }

    public createTopic() {
        this.store.dispatch(
            new CreateTopic({ name: this.translate.instant('topic') }),
        );
    }

    private sessionLoaded() {
        const currentSession = this.store.selectSnapshot(
            SessionsSelectors.currentSession(),
        );
        if (currentSession == null) {
            return;
        }

        this.updateBreadcrumbs([
            { label: 'sessions', path: 'sessions' },
            { label: currentSession.name },
        ]);
    }
}
