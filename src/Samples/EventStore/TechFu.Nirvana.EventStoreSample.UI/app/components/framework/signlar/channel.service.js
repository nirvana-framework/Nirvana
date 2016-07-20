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
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
var core_1 = require("@angular/core");
var Subject_1 = require("rxjs/Subject");
var Common_1 = require("../../../models/CQRS/Common");
var ChannelService = (function () {
    function ChannelService(window, channelConfig) {
        var _this = this;
        this.window = window;
        this.channelConfig = channelConfig;
        this.connectionStateSubject = new Subject_1.Subject();
        this.startingSubject = new Subject_1.Subject();
        this.errorSubject = new Subject_1.Subject();
        this.subjects = new Array();
        if (this.window.$ === undefined || this.window.$.hubConnection === undefined) {
            throw new Error("The variable '$' or the .hubConnection() function are not defined...please check the SignalR scripts have been loaded properly");
        }
        this.connectionState$ = this.connectionStateSubject.asObservable();
        this.error$ = this.errorSubject.asObservable();
        this.starting$ = this.startingSubject.asObservable();
        this.hubConnection = this.window.$.hubConnection();
        this.hubConnection.url = channelConfig.url;
        this.hubProxy = this.hubConnection.createHubProxy(channelConfig.hubName);
        this.hubConnection.stateChanged(function (state) {
            var newState = Common_1.ConnectionState.Connecting;
            switch (state.newState) {
                case _this.window.$.signalR.connectionState.connecting:
                    newState = Common_1.ConnectionState.Connecting;
                    break;
                case _this.window.$.signalR.connectionState.connected:
                    newState = Common_1.ConnectionState.Connected;
                    break;
                case _this.window.$.signalR.connectionState.reconnecting:
                    newState = Common_1.ConnectionState.Reconnecting;
                    break;
                case _this.window.$.signalR.connectionState.disconnected:
                    newState = Common_1.ConnectionState.Disconnected;
                    break;
            }
            _this.connectionStateSubject.next(newState);
        });
        this.hubConnection.error(function (error) {
            _this.errorSubject.next(error);
        });
        this.hubProxy.on("onEvent", function (channel, ev) {
            var channelSub = _this.subjects.find(function (x) {
                return x.channel === channel;
            });
            if (channelSub !== undefined) {
                return channelSub.subject.next(ev);
            }
        });
    }
    ChannelService.prototype.start = function () {
        var _this = this;
        this.hubConnection.start()
            .done(function () {
            _this.startingSubject.next({});
        })
            .fail(function (error) {
            _this.startingSubject.error(error);
        });
    };
    ChannelService.prototype.sub = function (channel) {
        var _this = this;
        var channelSub = this.subjects.find(function (x) {
            return x.channel === channel;
        });
        if (channelSub !== undefined) {
            console.log("Found existing observable for " + channel + " channel");
            return channelSub.subject.asObservable();
        }
        channelSub = new Common_1.ChannelSubject();
        channelSub.channel = channel;
        channelSub.subject = new Subject_1.Subject();
        this.subjects.push(channelSub);
        this.starting$.subscribe(function () {
            _this.hubProxy.invoke("Subscribe", channel)
                .done(function () {
                console.log("Successfully subscribed to " + channel + " channel");
            })
                .fail(function (error) {
                channelSub.subject.error(error);
            });
        }, function (error) {
            channelSub.subject.error(error);
        });
        return channelSub.subject.asObservable();
    };
    ChannelService.prototype.publish = function (ev) {
        this.hubProxy.invoke("Publish", ev);
    };
    ChannelService = __decorate([
        core_1.Injectable(),
        __param(0, core_1.Inject(Common_1.SignalrWindow)),
        __param(1, core_1.Inject("channel.config")), 
        __metadata('design:paramtypes', [Common_1.SignalrWindow, Common_1.ChannelConfig])
    ], ChannelService);
    return ChannelService;
}());
exports.ChannelService = ChannelService;
//# sourceMappingURL=channel.service.js.map