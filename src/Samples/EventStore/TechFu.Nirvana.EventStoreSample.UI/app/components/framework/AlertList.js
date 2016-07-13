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
var core_1 = require("@angular/core");
var ng2_bootstrap_1 = require("ng2-bootstrap/ng2-bootstrap");
var Commands_1 = require("../../models/CQRS/Commands");
var ServerMessageListComponenet = (function () {
    function ServerMessageListComponenet() {
        this.infoMessages = [];
        this.warningMessages = [];
        this.errorMessages = [];
        this.exceptionMessages = [];
    }
    ServerMessageListComponenet.prototype.clearAll = function () {
        this.warningMessages = [];
        this.infoMessages = [];
        this.errorMessages = [];
        this.exceptionMessages = [];
    };
    ServerMessageListComponenet.prototype.setMessages = function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            if (message.Key == "") {
                switch (message.MessageType) {
                    case Commands_1.MessageType.Error:
                        this.errorMessages.push(message);
                        break;
                    case Commands_1.MessageType.Exception:
                        this.exceptionMessages.push(message);
                        break;
                    case Commands_1.MessageType.Warning:
                        this.warningMessages.push(message);
                        break;
                    case Commands_1.MessageType.Info:
                        this.infoMessages.push(message);
                        break;
                }
            }
        }
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ServerMessageListComponenet.prototype, "infoMessages", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ServerMessageListComponenet.prototype, "warningMessages", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ServerMessageListComponenet.prototype, "errorMessages", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Array)
    ], ServerMessageListComponenet.prototype, "exceptionMessages", void 0);
    ServerMessageListComponenet = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'server-message-list',
            templateUrl: 'alerts.html',
            directives: [ng2_bootstrap_1.AlertComponent]
        }), 
        __metadata('design:paramtypes', [])
    ], ServerMessageListComponenet);
    return ServerMessageListComponenet;
}());
exports.ServerMessageListComponenet = ServerMessageListComponenet;
//# sourceMappingURL=AlertList.js.map