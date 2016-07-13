"use strict";
var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
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
var basePage_1 = require("./basePage");
var errorrService_1 = require("../../services/errorrService");
var ng2_bootstrap_1 = require('ng2-bootstrap/ng2-bootstrap');
var router_1 = require('@angular/router');
var AlertList_1 = require("../framework/AlertList");
var serverService_1 = require("../../services/serverService");
var SearchResultsComponent = (function (_super) {
    __extends(SearchResultsComponent, _super);
    function SearchResultsComponent(_securityService, errorService, router, route) {
        _super.call(this, _securityService, errorService, route);
        this.router = router;
        this.componentName = 'searchResults';
    }
    SearchResultsComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.page = 1;
        this.itemsPerPage = 20;
        this.registerEvents(this.errorList);
        this.keyParameterSub = this.route.params.subscribe(function (params) {
            _this.searchTerm = params['searchTerm'];
            _this.doClientSearch();
        });
    };
    SearchResultsComponent.prototype.ngOnDestroy = function () {
        this.disposeRegisteredEvents();
    };
    SearchResultsComponent.prototype.pageChanged = function (event) {
        this.page = event.page;
        this.doClientSearch();
    };
    ;
    SearchResultsComponent.prototype.doClientSearch = function () {
        console.log("do Search");
    };
    __decorate([
        core_1.ViewChild(AlertList_1.ServerMessageListComponenet), 
        __metadata('design:type', AlertList_1.ServerMessageListComponenet)
    ], SearchResultsComponent.prototype, "errorList", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', String)
    ], SearchResultsComponent.prototype, "searchTerm", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], SearchResultsComponent.prototype, "page", void 0);
    __decorate([
        core_1.Input(), 
        __metadata('design:type', Number)
    ], SearchResultsComponent.prototype, "itemsPerPage", void 0);
    SearchResultsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'search-component',
            templateUrl: 'search.html',
            directives: [ng2_bootstrap_1.PAGINATION_DIRECTIVES],
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, errorrService_1.ErrorService, router_1.Router, router_1.ActivatedRoute])
    ], SearchResultsComponent);
    return SearchResultsComponent;
}(basePage_1.BasePage));
exports.SearchResultsComponent = SearchResultsComponent;
//# sourceMappingURL=searchResultsComponent.js.map