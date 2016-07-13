"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var serverService_1 = require('../../services/serverService');
var basePage_1 = require("./basePage");
var errorrService_1 = require("../../services/errorrService");
var AlertList_1 = require("../framework/AlertList");
var Commands_1 = require("../../models/CQRS/Commands");
var DashboardComponent = (function (_super) {
    __extends(DashboardComponent, _super);
    function DashboardComponent(_securityService, errorService) {
        _super.call(this, _securityService, errorService, null);
        this.lastClickTime = null;
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
    }
    DashboardComponent.prototype.ngOnInit = function () {
    };
    DashboardComponent.prototype.ngOnDestroy = function () {
        this.disposeRegisteredEvents();
    };
    DashboardComponent.prototype.sendCommand = function () {
        var _this = this;
        this._serverService.mediator.command(new Commands_1.TestCommand())
            .then(function (x) { return _this.showClicked(); });
    };
    DashboardComponent.prototype.showClicked = function () {
        this.lastClickTime = new Date();
    };
    __decorate([
        core_1.ViewChild(AlertList_1.ServerMessageListComponenet), 
        __metadata('design:type', AlertList_1.ServerMessageListComponenet)
    ], DashboardComponent.prototype, "errorList", void 0);
    DashboardComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'dashboard-component',
            templateUrl: 'dashboard.html',
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, errorrService_1.ErrorService])
    ], DashboardComponent);
    return DashboardComponent;
}(basePage_1.BasePage));
exports.DashboardComponent = DashboardComponent;
//# sourceMappingURL=dashboard.component.js.map