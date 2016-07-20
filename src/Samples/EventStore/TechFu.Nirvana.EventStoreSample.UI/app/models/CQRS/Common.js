"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var ServerException = (function () {
    function ServerException() {
    }
    return ServerException;
}());
exports.ServerException = ServerException;
var PagedResult = (function () {
    function PagedResult() {
    }
    return PagedResult;
}());
exports.PagedResult = PagedResult;
var Command = (function () {
    function Command(endpointName) {
        this.endpointName = endpointName;
    }
    return Command;
}());
exports.Command = Command;
var Query = (function () {
    function Query(endpointName) {
        this.endpointName = endpointName;
    }
    return Query;
}());
exports.Query = Query;
var Response = (function () {
    function Response() {
    }
    Response.prototype.Success = function () {
        return this.IsValid && this.Exception == null;
    };
    return Response;
}());
exports.Response = Response;
var CommandResponse = (function (_super) {
    __extends(CommandResponse, _super);
    function CommandResponse() {
        _super.apply(this, arguments);
    }
    CommandResponse.prototype.Throw = function () {
        if (this.Exception) {
            throw this.Exception.Message;
        }
    };
    return CommandResponse;
}(Response));
exports.CommandResponse = CommandResponse;
var QueryResponse = (function (_super) {
    __extends(QueryResponse, _super);
    function QueryResponse() {
        _super.apply(this, arguments);
    }
    QueryResponse.prototype.Throw = function () {
        if (this.Exception) {
            throw this.Exception.Message;
        }
    };
    return QueryResponse;
}(Response));
exports.QueryResponse = QueryResponse;
var SignalrWindow = (function (_super) {
    __extends(SignalrWindow, _super);
    function SignalrWindow() {
        _super.apply(this, arguments);
    }
    return SignalrWindow;
}(Window));
exports.SignalrWindow = SignalrWindow;
(function (ConnectionState) {
    ConnectionState[ConnectionState["Connecting"] = 1] = "Connecting";
    ConnectionState[ConnectionState["Connected"] = 2] = "Connected";
    ConnectionState[ConnectionState["Reconnecting"] = 3] = "Reconnecting";
    ConnectionState[ConnectionState["Disconnected"] = 4] = "Disconnected";
})(exports.ConnectionState || (exports.ConnectionState = {}));
var ConnectionState = exports.ConnectionState;
var ChannelConfig = (function () {
    function ChannelConfig() {
    }
    return ChannelConfig;
}());
exports.ChannelConfig = ChannelConfig;
var ChannelEvent = (function () {
    function ChannelEvent() {
        this.Timestamp = new Date();
    }
    return ChannelEvent;
}());
exports.ChannelEvent = ChannelEvent;
var ChannelSubject = (function () {
    function ChannelSubject() {
    }
    return ChannelSubject;
}());
exports.ChannelSubject = ChannelSubject;
//# sourceMappingURL=Common.js.map