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
var http_1 = require('@angular/http');
var router_1 = require('@angular/router');
require('rxjs/add/operator/toPromise');
var cookieWrapper_1 = require('./util/cookieWrapper');
var user_1 = require('../models/security/user');
var masthead_1 = require('../models/common/masthead');
var apiService_1 = require('./apiService');
var serializer_1 = require("./util/serializer");
var Common_1 = require("../models/CQRS/Common");
var Commands_1 = require("../models/CQRS/Commands");
var errorrService_1 = require("./errorrService");
var ServerService = (function () {
    function ServerService(router, http, cookies, errorService, constants) {
        this.router = router;
        this.http = http;
        this.cookies = cookies;
        this.errorService = errorService;
        this.constants = constants;
        this.currentUser = new user_1.User();
        this.masthead = new masthead_1.MastHead();
        this.authKeys = {};
        this.onLogin = new core_1.EventEmitter();
        this.serializer = new serializer_1.Serializer();
        this.mediator = new apiService_1.Mediator(this.http, this, this.serializer);
        this.masthead.setTitle('Nirvana Event Store Sample');
        this.authKeys.loginCookie = "loginCookie";
        this.authKeys.sessionCookie = "sessionCookie";
        //Get this from cookie later...
        this.sessionId = this.getSessionId();
    }
    ServerService.prototype.setSession = function (session) {
        this.session = session.Result;
        this.sessionId = this.session.Id;
        var expiration = new Date();
        expiration.setDate(expiration.getDate() + 30);
        this.cookies.setCookie(this.authKeys.sessionCookie, this.session, expiration);
    };
    ServerService.prototype.handleError = function (componentName, error) {
        if (error) {
            var errors = [];
            this.errorService.showErrors(componentName, errors);
        }
        else {
            this.errorService.showErrors(componentName, [new Commands_1.ValidationMessage(Commands_1.MessageType.Exception, "", "There was a problem logging you in, please try again.")]);
        }
    };
    ;
    ServerService.prototype.checkLoggedIn = function () {
        return this.currentUser.loggedIn();
    };
    ServerService.prototype.getCurrentUser = function () {
        return this.currentUser;
    };
    ServerService.prototype.getToken = function () {
        return this.currentUser.token;
    };
    ServerService.prototype.getMastHead = function () {
        return this.masthead;
    };
    ServerService.prototype.setToken = function (token) {
        this.currentUser.token = token;
    };
    ServerService.prototype.getLoginEvent = function () {
        return this.onLogin;
    };
    ServerService.prototype.logOut = function () {
        this.cookies.removeCookie(this.authKeys.loginCookie);
        this.currentUser.logOut();
        this.onLogin.emit(this.currentUser.loggedIn());
    };
    ServerService.prototype.getSessionId = function () {
        var session = this.cookies.getCookie(this.authKeys.sessionCookie, new Commands_1.SessionViewModel());
        if (session.Id == "") {
            return this.constants.EmptyGuid;
        }
        return session.Id;
    };
    ServerService = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [router_1.Router, http_1.Http, cookieWrapper_1.CookieWrapper, errorrService_1.ErrorService, Common_1.AppConstants])
    ], ServerService);
    return ServerService;
}());
exports.ServerService = ServerService;
//# sourceMappingURL=serverService.js.map