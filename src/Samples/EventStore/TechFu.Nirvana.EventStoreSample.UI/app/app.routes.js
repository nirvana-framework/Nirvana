"use strict";
var router_1 = require('@angular/router');
var dashboard_component_1 = require("./components/pages/dashboard.component");
var searchResultsComponent_1 = require("./components/pages/searchResultsComponent");
var routes = [
    { path: '', component: dashboard_component_1.DashboardComponent },
    { path: 'searchResults/:searchTerm', component: searchResultsComponent_1.SearchResultsComponent },
];
exports.APP_ROUTER_PROVIDERS = [
    router_1.provideRouter(routes)
];
//# sourceMappingURL=app.routes.js.map