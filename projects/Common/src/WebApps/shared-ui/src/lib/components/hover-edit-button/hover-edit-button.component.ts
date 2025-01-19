import { Component, output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { HoverButtonDirective } from '../hover-button/hover-button.directive';

@Component({
    selector: 'shared-hover-edit-button',
    standalone: true,
    styles: `
        button.btn {
            --bs-btn-padding-x: 0;
            --bs-btn-padding-y: 0;
            font-size: inherit;
            margin-left: 0.75rem;
            height: 1em;
            position: relative;
            top: -0.5em;
        }
    `,
    imports: [FontAwesomeModule, HoverButtonDirective],
    template: `<button
        type="button"
        class="btn btn-link"
        (click)="(click)"
        sharedHoverButton
    >
        <fa-icon [icon]="['fas', 'edit']"></fa-icon>
    </button>`,
})
export class HoverEditButtonComponent {
    public click = output<void>();
}
