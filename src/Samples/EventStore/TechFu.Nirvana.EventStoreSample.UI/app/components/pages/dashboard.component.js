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
var router_1 = require("@angular/router");
var channel_service_1 = require("../framework/signlar/channel.service");
var DashboardComponent = (function (_super) {
    __extends(DashboardComponent, _super);
    function DashboardComponent(_securityService, errorService, channelService) {
        var _this = this;
        _super.call(this, _securityService, errorService, null);
        this.channelService = channelService;
        this.channel = "tasks";
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
        this.sentMessage = '';
        this.receivedMessage = '';
        this.channelService.sub(this.channel).subscribe(function (x) {
            switch (x.Name) {
                case 'ProductCatalog::HomePageUpdatedUiEvent': {
                    _this.refreshHomepageView();
                }
                case 'Infrastructure::TestUiEvent': {
                    _this.appendStatusUpdate(x);
                }
            }
        }, function (error) {
            console.warn("Attempt to join channel failed!", error);
        });
    }
    DashboardComponent.prototype.ngOnInit = function () {
        this.getSession();
        this.refreshHomepageView();
        this.refreshCart();
    };
    DashboardComponent.prototype.ngOnDestroy = function () {
        this.disposeRegisteredEvents();
    };
    DashboardComponent.prototype.refreshHomepageView = function () {
        var _this = this;
        this._serverService.mediator.query(new Commands_1.GetHomepageCataglogViewModelQuery()).then(function (x) { return _this.setHomePageModel(x); });
    };
    DashboardComponent.prototype.setHomePageModel = function (model) {
        this.model = model.Result;
    };
    DashboardComponent.prototype.refreshCart = function () {
        var _this = this;
        this._serverService.mediator.query(new Commands_1.GetCartViewModelQuery(this._serverService.sessionId, true))
            .then(function (x) { return _this.setCartViewModel(x); });
    };
    DashboardComponent.prototype.setCartViewModel = function (cart) {
        if (cart.IsValid) {
            this.cart = cart.Result;
        }
        else {
            this.cart = new Commands_1.CartViewModel();
        }
    };
    DashboardComponent.prototype.addToCart = function (productId) {
        this._serverService.mediator.command(new Commands_1.AddProductToCartCommand(this._serverService.sessionId, productId, 1));
    };
    DashboardComponent.prototype.createTestCatalog = function () {
        this._serverService.mediator.command(new Commands_1.CreateSampleCatalogCommand());
    };
    DashboardComponent.prototype.sendCommand = function () {
        var _this = this;
        this._serverService.mediator.command(new Commands_1.TestCommand())
            .then(function (x) { return _this.showClicked(); });
    };
    DashboardComponent.prototype.showClicked = function () {
        this.sentMessage += new Date().toDateString() + '\n\n';
    };
    DashboardComponent.prototype.appendStatusUpdate = function (ev) {
        this.receivedMessage += (new Date().toLocaleTimeString() + " : ") + JSON.stringify(ev.Data) + '\n\n';
    };
    DashboardComponent.prototype.clear = function () {
        this.sentMessage = '';
        this.receivedMessage = '';
    };
    DashboardComponent.prototype.getSession = function () {
        var _this = this;
        this._serverService.mediator.query(new Commands_1.GetNewSessionViewModelQuery(this._serverService.sessionId, true))
            .then(function (x) { return _this._serverService.setSession(x); });
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
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, errorrService_1.ErrorService, channel_service_1.ChannelService])
    ], DashboardComponent);
    return DashboardComponent;
}(basePage_1.BasePage));
exports.DashboardComponent = DashboardComponent;
//# sourceMappingURL=dashboard.component.js.map