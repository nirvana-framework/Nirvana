"use strict";
var MastHead = (function () {
    function MastHead() {
        this.title = 'Nirvana Event Store Sample';
        this.logoutMessage = 'Log Out';
    }
    MastHead.prototype.setTitle = function (title) {
        this.title = title;
    };
    MastHead.prototype.setWelcomeMessage = function (user) {
        this.welcomeMessage = user.userName;
    };
    return MastHead;
}());
exports.MastHead = MastHead;
//# sourceMappingURL=masthead.js.map