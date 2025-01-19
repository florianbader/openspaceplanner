import { Component, input, output } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { IconProp } from '@fortawesome/angular-fontawesome/types';

@Component({
    standalone: true,
    imports: [FontAwesomeModule],
    selector: 'shared-floating-action-button-item',
    templateUrl: 'floating-action-button-item.component.html',
    styleUrl: 'floating-action-button-item.component.scss',
})
export class FloatingActionButtonItemComponent {
    public icon = input.required<IconProp>();
    public click = output<void>();
}
