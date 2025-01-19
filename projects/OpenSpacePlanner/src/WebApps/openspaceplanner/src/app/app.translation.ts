import { HttpBackend, HttpClient } from '@angular/common/http';
import { TranslateLoader } from '@ngx-translate/core';
import { Observable } from 'rxjs';
import { environment } from '../environments/environment';

export class ApiTranslateLoader implements TranslateLoader {
    private readonly http: HttpClient;

    constructor(http: HttpBackend) {
        this.http = new HttpClient(http);
    }

    public getTranslation(lang: string): Observable<any> {
        return this.http.get(
            `${environment.backendApiBaseUrl}/api/i18n/translations/${lang}`,
        );
    }
}

// Using HttpBackend to create a new HttpClient instance without interceptors.
export function HttpLoaderFactory(http: HttpBackend): TranslateLoader {
    return new ApiTranslateLoader(http);
}

export const provideTranslation = () => ({
    defaultLanguage: 'en',
    loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpBackend],
    },
});
