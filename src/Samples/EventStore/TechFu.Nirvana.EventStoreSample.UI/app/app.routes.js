"use strict";
var router_1 = require('@angular/router');
var dashboard_component_1 = require("./components/pages/dashboard.component");
var LeadTestComponent_1 = require("./components/pages/LeadTestComponent");
var routes = [
    { path: '', component: dashboard_component_1.DashboardComponent },
    { path: 'leads', component: LeadTestComponent_1.LeadTestComponent },
];
exports.APP_ROUTER_PROVIDERS = [
    router_1.provideRouter(routes)
];
//# sourceMappingURL=app.routes.js.map