import { provideHttpClient } from '@angular/common/http';
import {
    ApplicationConfig,
    importProvidersFrom,
    provideExperimentalZonelessChangeDetection,
} from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { withNgxsReduxDevtoolsPlugin } from '@ngxs/devtools-plugin';
import { withNgxsLoggerPlugin } from '@ngxs/logger-plugin';
import {
    NavigationActionTiming,
    withNgxsRouterPlugin,
} from '@ngxs/router-plugin';
import {
    NoopNgxsExecutionStrategy,
    provideStore,
    withNgxsDevelopmentOptions,
} from '@ngxs/store';
import {
    Configuration as BackendApiConfiguration,
    ApiModule as BackendApiModule,
} from '@rio-scaffolding/shared-backend-api';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { environment } from '../environments/environment';
import { AppState } from './app-state/app.state';
import { appRoutes } from './app.routes';
import { provideTranslation } from './app.translation';

export const appConfig: ApplicationConfig = {
    providers: [
        provideExperimentalZonelessChangeDetection(),
        provideRouter(appRoutes, withComponentInputBinding()),
        provideHttpClient(),
        provideStore(
            [AppState],
            {
                developmentMode: !environment.production,
                executionStrategy: NoopNgxsExecutionStrategy,
            },
            withNgxsDevelopmentOptions({
                warnOnUnhandledActions: true,
            }),
            withNgxsRouterPlugin({
                navigationActionTiming: NavigationActionTiming.PostActivation,
            }),
            withNgxsLoggerPlugin({
                disabled: environment.production,
            }),
            withNgxsReduxDevtoolsPlugin({
                disabled: environment.production,
            }),
        ),
        importProvidersFrom(
            TranslateModule.forRoot(provideTranslation()),
            BackendApiModule.forRoot(
                () =>
                    new BackendApiConfiguration({
                        basePath: environment.backendApiBaseUrl,
                    }),
            ),
            NgxUiLoaderModule.forRoot({
                hasProgressBar: true,
                delay: 700, // show loader if it takes longer than delay in ms
            }),
        ),
    ],
};
