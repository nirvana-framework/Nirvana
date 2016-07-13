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
var serializer_1 = require("../../services/util/serializer");
var Commands_1 = require("../../models/CQRS/Commands");
var core_1 = require("@angular/core");
var async_1 = require("@angular/common/src/facade/async");
var BasePage = (function () {
    function BasePage(_serverService, errors, route) {
        this._serverService = _serverService;
        this.errors = errors;
        this.route = route;
        this.onMessagesReceoved = new async_1.EventEmitter();
        this.serializer = new serializer_1.Serializer();
        this.mediator = this._serverService.mediator;
        this.generalMessages = [];
    }
    BasePage.prototype.receiveErrors = function (messages) {
        this.generalMessages = messages;
        this.onMessagesReceoved.emit(this.generalMessages);
    };
    BasePage.prototype.registerEvents = function (alertComponent) {
        var _this = this;
        var emitter = this.errors.registerComponent(this.componentName);
        this.validationMessageSubscription = emitter.subscribe(function (x) { return _this.receiveErrors(x); });
        this.onMessagesReceoved.subscribe(function (x) { return _this.updateAlertComponent(alertComponent, x); });
    };
    BasePage.prototype.disposeRegisteredEvents = function () {
        this.errors.unregisterComponent(this.componentName);
        this.onMessagesReceoved.unsubscribe();
        if (this.keyParameterSub) {
            this.keyParameterSub.unsubscribe();
        }
    };
    BasePage.prototype.showSuccess = function () {
        this.showInfo("Your Changes were saved");
    };
    BasePage.prototype.showInfo = function (message) {
        this.errors.showErrors(this.componentName, [new Commands_1.ValidationMessage(Commands_1.MessageType.Info, "", message)]);
    };
    BasePage.prototype.showWarning = function (message) {
        this.errors.showErrors(this.componentName, [new Commands_1.ValidationMessage(Commands_1.MessageType.Warning, "", message)]);
    };
    BasePage.prototype.showError = function (message) {
        this.errors.showErrors(this.componentName, [new Commands_1.ValidationMessage(Commands_1.MessageType.Error, "", message)]);
    };
    BasePage.prototype.showException = function (message) {
        this.errors.showErrors(this.componentName, [new Commands_1.ValidationMessage(Commands_1.MessageType.Exception, "", message)]);
    };
    BasePage.prototype.updateAlertComponent = function (alertComponent, x) {
        alertComponent.clearAll();
        alertComponent.setMessages(x);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], BasePage.prototype, "loggedIn", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], BasePage.prototype, "componentName", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], BasePage.prototype, "generalMessages", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Object)
    ], BasePage.prototype, "validationMessageSubscription", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', async_1.EventEmitter)
    ], BasePage.prototype, "onMessagesReceoved", void 0);
    return BasePage;
}());
exports.BasePage = BasePage;
//# sourceMappingURL=basePage.js.map