"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var common_1 = require('@angular/common');
var router_1 = require('@angular/router');
var core_2 = require("angular2-cookie/core");
var header_component_1 = require('./components/header.component');
var nav_component_1 = require('./components/nav.component');
var serverService_1 = require("./services/serverService");
var cookieWrapper_1 = require("./services/util/cookieWrapper");
var errorrService_1 = require("./services/errorrService");
require('rxjs/add/operator/toPromise');
var AppComponent = (function () {
    function AppComponent(secService, viewContainerRef) {
        this.secService = secService;
        this.viewContainerRef = viewContainerRef;
    }
    AppComponent.prototype.ngOnInit = function () {
    };
    AppComponent = __decorate([
        core_1.Component({
            selector: 'my-app',
            moduleId: module.id,
            templateUrl: 'app.html',
            directives: [header_component_1.HeaderComponent, nav_component_1.NavComponent, router_1.ROUTER_DIRECTIVES],
            providers: [common_1.Location, cookieWrapper_1.CookieWrapper, serverService_1.ServerService, core_2.CookieService, errorrService_1.ErrorService],
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, core_1.ViewContainerRef])
    ], AppComponent);
    return AppComponent;
}());
exports.AppComponent = AppComponent;
//# sourceMappingURL=app.component.js.map