import {
    Component,
    inject,
    input,
    output,
    TemplateRef,
    ViewChild,
} from '@angular/core';
import { NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule } from '@ngx-translate/core';

@Component({
    standalone: true,
    imports: [TranslateModule],
    selector: 'shared-off-canvas',
    templateUrl: 'off-canvas.component.html',
})
export class OffCanvasComponent {
    private readonly offCanvasService = inject(NgbOffcanvas);

    public save = output<void>();
    public title = input<string>();

    @ViewChild('content', { static: true })
    public content: TemplateRef<any> | null = null;

    public open() {
        this.offCanvasService.open(this.content, {
            position: 'end',
            scroll: true,
            container: 'body',
        });
    }
}
