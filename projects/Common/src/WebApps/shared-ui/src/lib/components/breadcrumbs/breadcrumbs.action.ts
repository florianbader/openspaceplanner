import { Breadcrumb } from './breadcrumb';

export class UpdateBreadcrumbs {
    public static readonly type = '[App] Update Breadcrumbs';

    constructor(public breadcrumbs: Breadcrumb[]) {}
}
