
import {provideRouter, RouterConfig} from '@angular/router';
import {DashboardComponent} from "./components/pages/dashboard.component";
import {SearchResultsComponent} from "./components/pages/searchResultsComponent";

const routes:RouterConfig = [
    {path: '', component: DashboardComponent},
    {path: 'searchResults/:searchTerm', component: SearchResultsComponent},
];

export const APP_ROUTER_PROVIDERS = [
    provideRouter(routes)
];
