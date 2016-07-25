
import {provideRouter, RouterConfig} from '@angular/router';
import {DashboardComponent} from "./components/pages/dashboard.component";

const routes:RouterConfig = [
    {path: '', component: DashboardComponent},
];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];
