import { Route } from '@angular/router';
import { provideStates } from '@ngxs/store';
import { SessionComponent } from './session/session.component';
import { SessionsState } from './sessions-state/sessions.state';
import { SessionsComponent } from './sessions.component';

export const sessionsRoutes: Route[] = [
    {
        path: '',
        providers: [provideStates([SessionsState])],
        children: [
            {
                path: '',
                component: SessionsComponent,
                providers: [provideStates([SessionsState])],
            },
            {
                path: ':sessionId',
                component: SessionComponent,
            },
            {
                path: '**',
                redirectTo: '',
            },
        ],
    },
];
