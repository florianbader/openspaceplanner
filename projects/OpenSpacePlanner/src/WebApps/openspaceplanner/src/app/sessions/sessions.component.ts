import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap';
import { select } from '@ngxs/store';
import { ComponentBase } from '@rio-scaffolding/shared-ui';
import { takeUntil } from 'rxjs';
import { CreateSession } from './sessions-state/actions/create-session';
import { GetSessions } from './sessions-state/actions/get-sessions';
import { SessionsSelectors } from './sessions-state/sessions.state.selectors';

@Component({
    standalone: true,
    imports: [RouterLink, NgbPagination],
    selector: 'app-sessions',
    templateUrl: 'sessions.component.html',
})
export class SessionsComponent extends ComponentBase implements OnInit {
    public sessions = select(SessionsSelectors.sessions());
    public pageResult = select(SessionsSelectors.pageResult());

    public ngOnInit() {
        this.updateBreadcrumbs([{ label: 'sessions' }]);

        this.addLoader([GetSessions, CreateSession]);
        this.store.dispatch(new GetSessions());
    }

    public createSession() {
        this.store
            .dispatch(
                new CreateSession({ name: this.translate.instant('session') }),
            )
            .pipe(takeUntil(this.destroy$))
            .subscribe(() => {
                const currentSession = this.store.selectSnapshot(
                    SessionsSelectors.currentSession(),
                );
                if (currentSession == null) {
                    return;
                }

                this.navigate(['sessions', currentSession.id]);
            });
    }

    public pageChange(page: number) {
        this.store.dispatch(new GetSessions(page));
    }
}
