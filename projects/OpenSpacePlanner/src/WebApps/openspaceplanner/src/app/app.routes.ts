import { Route } from '@angular/router';

export const appRoutes: Route[] = [
    {
        path: 'sessions',
        loadChildren: () =>
            import('./sessions/sessions.routes').then(
                (feature) => feature.sessionsRoutes,
            ),
    },
    {
        path: '**',
        redirectTo: 'sessions',
    },
];
