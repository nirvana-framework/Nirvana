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
var core_2 = require("angular2-cookie/core");
var serializer_1 = require("./serializer");
var CookieWrapper = (function () {
    function CookieWrapper(cookies) {
        this.cookies = cookies;
        // private cookies:CookieService;
        this.serializer = new serializer_1.Serializer();
        // var options:CookieOptions = null;
        // this.cookies = new CookieService(options);
    }
    CookieWrapper.prototype.setCookie = function (key, value, expires) {
        var options = {
            expires: expires
        };
        var valueString = this.serializer.serialize(value);
        this.cookies.put(key, valueString, options);
    };
    CookieWrapper.prototype.getCookie = function (key, blank) {
        var valueString = this.cookies.get(key);
        if (!valueString) {
            return null;
        }
        var obj = this.serializer.deserialize(blank, valueString);
        return obj;
    };
    CookieWrapper.prototype.removeCookie = function (key) {
        this.cookies.remove(key);
    };
    CookieWrapper = __decorate([
        core_1.Injectable(), 
        __metadata('design:paramtypes', [core_2.CookieService])
    ], CookieWrapper);
    return CookieWrapper;
}());
exports.CookieWrapper = CookieWrapper;
//# sourceMappingURL=cookieWrapper.js.map