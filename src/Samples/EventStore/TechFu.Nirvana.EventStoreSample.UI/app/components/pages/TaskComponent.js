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
var http_1 = require("@angular/http");
var channel_service_1 = require("../framework/signlar/channel.service");
var StatusEvent = (function () {
    function StatusEvent() {
    }
    return StatusEvent;
}());
var TaskComponent = (function () {
    function TaskComponent(http, channelService) {
        this.http = http;
        this.channelService = channelService;
        this.messages = "";
        this.channel = "tasks";
    }
    TaskComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.channelService.sub(this.channel).subscribe(function (x) {
            switch (x.Name) {
                case 'Infrastructure::TestUiEvent': {
                    _this.appendStatusUpdate(x);
                }
            }
        }, function (error) {
            console.warn("Attempt to join channel failed!", error);
        });
    };
    TaskComponent.prototype.appendStatusUpdate = function (ev) {
        this.messages += (new Date().toLocaleTimeString() + " : ") + JSON.stringify(ev.Data);
    };
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], TaskComponent.prototype, "eventName", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], TaskComponent.prototype, "apiUrl", void 0);
    TaskComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'task',
            templateUrl: 'TaskComponent.html'
        }), 
        __metadata('design:paramtypes', [http_1.Http, channel_service_1.ChannelService])
    ], TaskComponent);
    return TaskComponent;
}());
exports.TaskComponent = TaskComponent;
//# sourceMappingURL=TaskComponent.js.map