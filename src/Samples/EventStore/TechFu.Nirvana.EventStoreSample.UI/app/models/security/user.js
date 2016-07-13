"use strict";
var User = (function () {
    function User() {
        this.userName = '';
        this.token = '';
        this.userName = '';
        this.token = '';
    }
    User.prototype.loggedIn = function () {
        return this.token != '';
    };
    ;
    User.prototype.logOut = function () {
        this.token = '';
        this.userName = '';
        this.scope = '';
    };
    return User;
}());
exports.User = User;
//# sourceMappingURL=user.js.map