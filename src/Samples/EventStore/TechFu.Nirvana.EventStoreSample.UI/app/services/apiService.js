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
var serverService_1 = require("./serverService");
var serializer_1 = require("./util/serializer");
var lang_1 = require("@angular/platform-browser-dynamic/src/facade/lang");
require('rxjs/add/operator/toPromise');
var Mediator = (function () {
    function Mediator(http, security, serializer) {
        this.http = http;
        this.security = security;
        this.serializer = serializer;
        this.commandEndpoint = 'https://local-commandapi.mean.software:54406/api';
        this.queryEndpoint = 'https://local-queryapi.mean.software:54406/api/';
    }
    Mediator.prototype.getCommandUrl = function (command) {
        var url = this.commandEndpoint + "/" + command.endpointName;
        return url;
    };
    Mediator.prototype.getQueryUrl = function (query) {
        var url = this.queryEndpoint + "/" + query.endpointName;
        return url;
    };
    Mediator.prototype.getHeaders = function () {
        var headers = new http_1.Headers({ 'Content-Type': 'application/json' });
        headers.append('Authorization', "Bearer " + this.security.getToken());
        return headers;
    };
    Mediator.prototype.query = function (query) {
        var url = this.getQueryUrl(query) + '?' + this.objectToParams(query);
        return this.http
            .get(url, { headers: this.getHeaders() })
            .toPromise()
            .then(function (res) { return res.json(); });
    };
    Mediator.prototype.command = function (command) {
        return this.http
            .post(this.getCommandUrl(command), this.serializer.serialize(command), { headers: this.getHeaders() })
            .toPromise()
            .then(function (res) { return res.json(); });
    };
    Mediator.prototype.objectToParams = function (object) {
        var _this = this;
        return Object.keys(object).map(function (key) { return _this.isEndPoint(key) ? '' : lang_1.isJsObject(object[key]) ?
            _this.subObjectToParams(encodeURIComponent(key), object[key])
            : encodeURIComponent(key) + "=" + _this.cleanseValue(object, key); }).join('&');
    };
    Mediator.prototype.cleanseValue = function (object, key) {
        if (object[key] == null) {
            return '';
        }
        return encodeURIComponent(object[key]);
    };
    Mediator.prototype.subObjectToParams = function (key, object) {
        var _this = this;
        return Object.keys(object)
            .map(function (childKey) { return _this.isEndPoint(key) ? '' : lang_1.isJsObject(object[childKey]) ?
            _this.subObjectToParams(key + "[" + encodeURIComponent(childKey) + "]", object[childKey])
            : key + "[" + encodeURIComponent(childKey) + "]=" + _this.cleanseValue(object, childKey); }).join('&');
    };
    //endpointName and typescriptPlace are used only for communication and TS copatability
    Mediator.prototype.isEndPoint = function (key) {
        return key == 'endpointName'
            || key == 'typescriptPlace';
    };
    Mediator = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [http_1.Http, serverService_1.ServerService, serializer_1.Serializer])
    ], Mediator);
    return Mediator;
}());
exports.Mediator = Mediator;
//# sourceMappingURL=apiService.js.map