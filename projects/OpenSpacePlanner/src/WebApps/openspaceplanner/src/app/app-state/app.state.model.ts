import { Breadcrumb } from '@rio-scaffolding/shared-ui';

export interface AppStateModel {
    tenant: string | null;
    breadcrumbs: Breadcrumb[];
}
