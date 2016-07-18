
import {provideRouter, RouterConfig} from '@angular/router';
import {DashboardComponent} from "./components/pages/dashboard.component";
import {TaskComponent} from "./components/pages/TaskComponent";

const routes:RouterConfig = [
    {path: '', component: DashboardComponent},
    {path: 'tasks', component: TaskComponent},
];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];
