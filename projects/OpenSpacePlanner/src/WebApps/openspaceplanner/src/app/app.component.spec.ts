import { ComponentFixture } from '@angular/core/testing';
import { render } from '@testing-library/angular';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
    let fixture: ComponentFixture<AppComponent>;

    beforeEach(async () => {
        const renderResult = await render(AppComponent);
        fixture = renderResult.fixture;

        await fixture.whenStable();
    });

    it('should render title', async () => {
        expect(document.querySelector('span.fs-4')?.textContent).toContain(
            'openspaceplanner',
        );
    });
});
