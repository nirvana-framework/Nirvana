
import {provideRouter, RouterConfig} from '@angular/router';
import {DashboardComponent} from "./components/pages/dashboard.component";
import {LeadTestComponent} from "./components/pages/LeadTestComponent";

const routes:RouterConfig = [
    {path: '', component: DashboardComponent},
    {path: 'leads', component: LeadTestComponent},
];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];
