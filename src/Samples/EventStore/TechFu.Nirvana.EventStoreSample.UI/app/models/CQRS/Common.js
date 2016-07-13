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
//# sourceMappingURL=Common.js.map