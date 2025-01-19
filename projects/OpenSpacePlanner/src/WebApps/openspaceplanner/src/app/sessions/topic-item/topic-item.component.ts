import { Component, input } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { TranslateModule } from '@ngx-translate/core';
import { Topic } from '@rio-scaffolding/shared-backend-api';
import { ComponentBase } from '@rio-scaffolding/shared-ui';

@Component({
    standalone: true,
    imports: [FontAwesomeModule, TranslateModule],
    selector: 'app-topic-item',
    templateUrl: 'topic-item.component.html',
    styleUrl: 'topic-item.component.scss',
})
export class TopicItemComponent extends ComponentBase {
    public topic = input.required<Topic>();
}
