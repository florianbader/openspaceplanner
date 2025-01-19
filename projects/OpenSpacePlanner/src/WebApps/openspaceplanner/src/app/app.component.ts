import { Component, OnInit } from '@angular/core';
import { RouterModule } from '@angular/router';
import {
    FaConfig,
    FaIconLibrary,
    FontAwesomeModule,
} from '@fortawesome/angular-fontawesome';
import { fab } from '@fortawesome/free-brands-svg-icons';
import { far } from '@fortawesome/free-regular-svg-icons';
import { faBan, fas } from '@fortawesome/free-solid-svg-icons';
import { TranslateModule } from '@ngx-translate/core';
import { select } from '@ngxs/store';
import { BreadcrumbsComponent } from '@rio-scaffolding/shared-ui';
import { NgxUiLoaderModule } from 'ngx-ui-loader';
import { AppSelectors } from './app-state/app.state.selectors';

@Component({
    standalone: true,
    imports: [
        RouterModule,
        FontAwesomeModule,
        TranslateModule,
        NgxUiLoaderModule,
        BreadcrumbsComponent,
    ],
    selector: 'app-root',
    templateUrl: './app.component.html',
})
export class AppComponent implements OnInit {
    public breadcrumbs = select(AppSelectors.breadcrumbs());

    constructor(
        private readonly iconLibrary: FaIconLibrary,
        private readonly iconConfig: FaConfig,
    ) {}

    public ngOnInit(): void {
        this.configureIcons();
    }

    private configureIcons() {
        this.iconLibrary.addIconPacks(fas, far, fab);
        this.iconConfig.fallbackIcon = faBan;
        this.iconConfig.defaultPrefix = 'fas';
        this.iconConfig.fixedWidth = true;
    }
}
