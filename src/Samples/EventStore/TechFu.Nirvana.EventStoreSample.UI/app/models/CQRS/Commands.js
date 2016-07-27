"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Common_1 = require("./Common");
//Common
var ValidationMessage = (function () {
    function ValidationMessage(MessageType, Key, Message) {
        this.MessageType = MessageType;
        this.Key = Key;
        this.Message = Message;
    }
    return ValidationMessage;
}());
exports.ValidationMessage = ValidationMessage;
var PaginationQuery = (function () {
    function PaginationQuery(PageNumber, ItemsPerPage) {
        this.PageNumber = PageNumber;
        this.ItemsPerPage = ItemsPerPage;
    }
    return PaginationQuery;
}());
exports.PaginationQuery = PaginationQuery;
(function (MessageType) {
    MessageType[MessageType["Info"] = 1] = "Info";
    MessageType[MessageType["Warning"] = 2] = "Warning";
    MessageType[MessageType["Error"] = 3] = "Error";
    MessageType[MessageType["Exception"] = 4] = "Exception";
})(exports.MessageType || (exports.MessageType = {}));
var MessageType = exports.MessageType;
//Infrastructure
var GetVersionQuery = (function (_super) {
    __extends(GetVersionQuery, _super);
    function GetVersionQuery() {
        _super.call(this, 'Infrastructure/GetVersion');
    }
    return GetVersionQuery;
}(Common_1.Query));
exports.GetVersionQuery = GetVersionQuery;
var VersionModel = (function () {
    function VersionModel() {
    }
    return VersionModel;
}());
exports.VersionModel = VersionModel;
var TestCommand = (function (_super) {
    __extends(TestCommand, _super);
    function TestCommand() {
        _super.call(this, 'Infrastructure/Test');
    }
    return TestCommand;
}(Common_1.Command));
exports.TestCommand = TestCommand;
var TestResult = (function () {
    function TestResult() {
    }
    return TestResult;
}());
exports.TestResult = TestResult;
//ProductCatalog
var CreateSampleCatalogCommand = (function (_super) {
    __extends(CreateSampleCatalogCommand, _super);
    function CreateSampleCatalogCommand() {
        _super.call(this, 'ProductCatalog/CreateSampleCatalog');
    }
    return CreateSampleCatalogCommand;
}(Common_1.Command));
exports.CreateSampleCatalogCommand = CreateSampleCatalogCommand;
(function (Nop) {
    Nop[Nop["NoValue"] = 0] = "NoValue";
})(exports.Nop || (exports.Nop = {}));
var Nop = exports.Nop;
//Users
//# sourceMappingURL=Commands.js.map