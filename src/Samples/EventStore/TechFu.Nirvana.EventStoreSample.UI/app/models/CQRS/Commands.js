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
//Lead
var GetLeadIndicatorOveriewQuery = (function (_super) {
    __extends(GetLeadIndicatorOveriewQuery, _super);
    function GetLeadIndicatorOveriewQuery(LeadId, typescriptPlace) {
        _super.call(this, 'Lead/GetLeadIndicatorOveriew');
        this.LeadId = LeadId;
        this.typescriptPlace = typescriptPlace;
    }
    return GetLeadIndicatorOveriewQuery;
}(Common_1.Query));
exports.GetLeadIndicatorOveriewQuery = GetLeadIndicatorOveriewQuery;
var LeadIndicatorViewModel = (function () {
    function LeadIndicatorViewModel() {
    }
    return LeadIndicatorViewModel;
}());
exports.LeadIndicatorViewModel = LeadIndicatorViewModel;
var BusinesssMeasureViewModel = (function () {
    function BusinesssMeasureViewModel() {
    }
    return BusinesssMeasureViewModel;
}());
exports.BusinesssMeasureViewModel = BusinesssMeasureViewModel;
(function (EntityTypeValue) {
    EntityTypeValue[EntityTypeValue["SoleProp"] = 1] = "SoleProp";
    EntityTypeValue[EntityTypeValue["LLC"] = 2] = "LLC";
})(exports.EntityTypeValue || (exports.EntityTypeValue = {}));
var EntityTypeValue = exports.EntityTypeValue;
var PerformanceIndicatorViewModel = (function () {
    function PerformanceIndicatorViewModel() {
    }
    return PerformanceIndicatorViewModel;
}());
exports.PerformanceIndicatorViewModel = PerformanceIndicatorViewModel;
var IndicatorValueViewModel = (function () {
    function IndicatorValueViewModel() {
    }
    return IndicatorValueViewModel;
}());
exports.IndicatorValueViewModel = IndicatorValueViewModel;
var IndicatorSource = (function () {
    function IndicatorSource() {
    }
    return IndicatorSource;
}());
exports.IndicatorSource = IndicatorSource;
var IndicatorType = (function () {
    function IndicatorType() {
    }
    return IndicatorType;
}());
exports.IndicatorType = IndicatorType;
//# sourceMappingURL=Commands.js.map