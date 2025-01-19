import { Directive, ElementRef, OnInit } from '@angular/core';

@Directive({
    selector: '[sharedHoverButtonContainer]',
    standalone: true,
    host: {
        '(mouseout)': 'changeVisibility(false)',
        '(mouseover)': 'changeVisibility(true)',
    },
})
export class HoverButtonContainerDirective {
    private element: HTMLElement;

    constructor(elementRef: ElementRef) {
        this.element = elementRef.nativeElement;
    }

    public changeVisibility(visible: boolean) {
        const hoverButtonElement = this.element.querySelector(
            '.hover-button',
        ) as HTMLElement;
        if (hoverButtonElement == null) {
            return;
        }

        hoverButtonElement.style.display = visible ? 'inline-block' : 'none';
    }
}

@Directive({ selector: '[sharedHoverButton]', standalone: true })
export class HoverButtonDirective implements OnInit {
    private element: HTMLElement;

    constructor(elementRef: ElementRef) {
        this.element = elementRef.nativeElement;
    }

    public ngOnInit() {
        this.element.classList.add('hover-button');
        this.element.style.display = 'none';
    }
}
