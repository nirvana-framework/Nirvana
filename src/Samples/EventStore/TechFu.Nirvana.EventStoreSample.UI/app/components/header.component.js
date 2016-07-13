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
var router_1 = require('@angular/router');
var serverService_1 = require('../services/serverService');
var HeaderComponent = (function () {
    function HeaderComponent(_securityService, router) {
        this._securityService = _securityService;
        this.router = router;
    }
    HeaderComponent.prototype.ngOnInit = function () {
        this.masthead = this._securityService.getMastHead();
    };
    HeaderComponent.prototype.doLogOut = function () {
        this._securityService.logOut();
        this.router.navigate(['/']);
    };
    HeaderComponent.prototype.loggedIn = function () {
        return this._securityService.getCurrentUser().loggedIn();
    };
    HeaderComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'header-component',
            templateUrl: 'header.html',
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, router_1.Router])
    ], HeaderComponent);
    return HeaderComponent;
}());
exports.HeaderComponent = HeaderComponent;
//# sourceMappingURL=header.component.js.map