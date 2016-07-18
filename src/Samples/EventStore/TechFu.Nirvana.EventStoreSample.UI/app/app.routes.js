"use strict";
var router_1 = require('@angular/router');
var dashboard_component_1 = require("./components/pages/dashboard.component");
var TaskComponent_1 = require("./components/pages/TaskComponent");
var routes = [
    { path: '', component: dashboard_component_1.DashboardComponent },
    { path: 'tasks', component: TaskComponent_1.TaskComponent },
];
exports.APP_ROUTER_PROVIDERS = [
    router_1.provideRouter(routes)
];
//# sourceMappingURL=app.routes.js.map