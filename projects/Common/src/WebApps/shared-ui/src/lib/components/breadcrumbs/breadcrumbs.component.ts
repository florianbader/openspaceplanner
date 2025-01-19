import { Component, Input } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TranslateModule } from '@ngx-translate/core';
import { Breadcrumb } from './breadcrumb';

@Component({
    standalone: true,
    imports: [RouterModule, FontAwesomeModule, TranslateModule],
    selector: 'shared-breadcrumbs',
    templateUrl: 'breadcrumbs.component.html',
})
export class BreadcrumbsComponent {
    @Input() public breadcrumbs: Breadcrumb[] | null = [];
}
