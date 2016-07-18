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
var serverService_1 = require('../services/serverService');
var router_1 = require('@angular/router');
var NavComponent = (function () {
    function NavComponent(_securityService, router, activeRoute) {
        this._securityService = _securityService;
        this.router = router;
        this.activeRoute = activeRoute;
    }
    NavComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.searchTerm = '';
        this.loggedIn = this._securityService.checkLoggedIn();
        this._securityService.getLoginEvent().subscribe(function (x) { return _this.loggedIn = x; });
        this.links = [];
    };
    NavComponent.prototype.doSearch = function () {
        this.router.navigate(['/searchResults', this.searchTerm]);
    };
    NavComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'nav-component',
            templateUrl: 'nav.html',
            directives: [router_1.ROUTER_DIRECTIVES, common_1.NgFor],
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, router_1.Router, router_1.ActivatedRoute])
    ], NavComponent);
    return NavComponent;
}());
exports.NavComponent = NavComponent;
var NavItem = (function () {
    function NavItem(route, text) {
        this.route = route;
        this.text = text;
    }
    return NavItem;
}());
exports.NavItem = NavItem;
//# sourceMappingURL=nav.component.js.map