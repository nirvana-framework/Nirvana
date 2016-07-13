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
var collections_1 = require("../models/common/collections");
var ErrorService = (function () {
    function ErrorService() {
        this.registeredComponents = new Array();
        this.errors = new collections_1.Dictionary();
    }
    ErrorService.prototype.registerComponent = function (componentName) {
        if (!this.errors.containsKey(componentName)) {
            this.errors.add(componentName, new core_1.EventEmitter());
        }
        else {
            console.log('dispose did not handle correctly, WTF mate?');
        }
        return this.errors.getValue(componentName);
    };
    ErrorService.prototype.unregisterComponent = function (errorComponentName) {
        if (!this.errors.containsKey(errorComponentName)) {
            console.log('init failed, WTF mate?');
            return;
        }
        this.errors.remove(errorComponentName);
    };
    ErrorService.prototype.showErrors = function (componentName, messages) {
        if (!this.errors.containsKey(componentName)) {
            for (var i = 0; i < messages.length; i++) {
                console.log(messages[i].Key + ": " + messages[i].Message);
            }
        }
        else {
            this.errors.getValue(componentName).emit(messages);
        }
    };
    ErrorService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [])
    ], ErrorService);
    return ErrorService;
}());
exports.ErrorService = ErrorService;
//# sourceMappingURL=errorrService.js.map