import { Component, inject, OnDestroy } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Navigate } from '@ngxs/router-plugin';
import {
    Actions,
    ActionType,
    ofActionCanceled,
    ofActionCompleted,
    ofActionDispatched,
    Store,
} from '@ngxs/store';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Subject, takeUntil } from 'rxjs';
import { Breadcrumb } from './breadcrumbs/breadcrumb';
import { UpdateBreadcrumbs } from './breadcrumbs/breadcrumbs.action';

@Component({
    template: '',
})
export abstract class ComponentBase implements OnDestroy {
    private readonly actions$ = inject(Actions);
    private readonly loaderService = inject(NgxUiLoaderService);

    protected readonly destroy$ = new Subject<void>();
    protected readonly store = inject(Store);
    protected readonly translate = inject(TranslateService);

    public ngOnDestroy(): void {
        this.stopLoaders();
        this.destroy$.next();
    }

    protected navigate(path: string[]) {
        this.store.dispatch(new Navigate(path));
    }

    protected updateBreadcrumbs(breadcrumbs: Breadcrumb[]) {
        this.store.dispatch(new UpdateBreadcrumbs(breadcrumbs));
    }

    protected addLoader(actions: ActionType[]) {
        for (const action of actions) {
            this.actions$
                .pipe(ofActionDispatched(action), takeUntil(this.destroy$))
                .subscribe(() => {
                    this.startLoader(action);
                });

            this.actions$
                .pipe(ofActionCompleted(action), takeUntil(this.destroy$))
                .subscribe(() => {
                    this.stopLoader(action);
                });

            this.actions$
                .pipe(ofActionCanceled(action), takeUntil(this.destroy$))
                .subscribe(() => {
                    this.stopLoader(action);
                });
        }
    }

    protected stopLoaders() {
        this.loaderService.stopAll();
    }

    private stopLoader(action: ActionType) {
        setTimeout(() => {
            this.loaderService.stop(action.type);
        });
    }

    private startLoader(action: ActionType) {
        setTimeout(() => {
            this.loaderService.start(action.type);
        });
    }
}
