import { Component } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
    NgbDropdownConfig,
    NgbDropdownModule,
} from '@ng-bootstrap/ng-bootstrap';

@Component({
    standalone: true,
    imports: [NgbDropdownModule, FontAwesomeModule],
    selector: 'shared-floating-action-button',
    templateUrl: 'floating-action-button.component.html',
    styleUrl: 'floating-action-button.component.scss',
})
export class FloatingActionButtonComponent {
    constructor(config: NgbDropdownConfig) {
        config.placement = 'top-end';
        config.autoClose = true;
    }
}
