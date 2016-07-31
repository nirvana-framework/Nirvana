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
var serverService_1 = require('../../services/serverService');
var basePage_1 = require("./basePage");
var errorrService_1 = require("../../services/errorrService");
var AlertList_1 = require("../framework/AlertList");
var Commands_1 = require("../../models/CQRS/Commands");
var router_1 = require("@angular/router");
var channel_service_1 = require("../framework/signlar/channel.service");
var LeadTestComponent = (function (_super) {
    __extends(LeadTestComponent, _super);
    function LeadTestComponent(_securityService, errorService, channelService) {
        _super.call(this, _securityService, errorService, null);
        this.channelService = channelService;
        this.channel = "tasks";
        this.lead = new Commands_1.LeadIndicatorViewModel();
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
        this.sentMessage = '';
        this.receivedMessage = '';
    }
    LeadTestComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._serverService.mediator.query(new Commands_1.GetLeadIndicatorOveriewQuery("", true))
            .then(function (x) { return _this.bindLead(x); });
    };
    LeadTestComponent.prototype.ngOnDestroy = function () {
        this.disposeRegisteredEvents();
    };
    LeadTestComponent.prototype.getSourceValue = function (type, source) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.IndicatorType.Value == type.Value) {
                for (var j = 0; j < indicator.AllValues.length; j++) {
                    if (indicator.AllValues[j].Source.Value == source.Value && indicator.AllValues[j].Type.Value == type.Value)
                        return this.getDisplayFor(indicator.AllValues[j]);
                }
            }
        }
        return "";
    };
    LeadTestComponent.prototype.isSelectedSource = function (indicatorType, source) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.SelectedValue != null && indicator.IndicatorType.Value == indicatorType.Value && indicator.SelectedValue.Source.Value == source.Value) {
                return true;
            }
        }
        return false;
    };
    LeadTestComponent.prototype.setSelectedValue = function (indicatorType, source) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.IndicatorType.Value != indicatorType.Value) {
                continue;
            }
            for (var j = 0; j < indicator.AllValues.length; j++)
                if (indicator.AllValues[j].Source.Value == source.Value) {
                    indicator.SelectedValue = indicator.AllValues[j];
                }
        }
        this.updateBusinessMeasures();
    };
    LeadTestComponent.prototype.updateBusinessMeasures = function () {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.SelectedValue == null) {
                continue;
            }
            if (this.isAddress(indicator.IndicatorType)) {
                this.lead.Address = indicator.SelectedValue.Value;
            }
            if (this.isAnnualRevenue(indicator.IndicatorType)) {
                this.lead.BusinessMeasure.AnnualRevenue = indicator.SelectedValue.Value;
            }
            if (this.isEmployeeCount(indicator.IndicatorType)) {
                this.lead.BusinessMeasure.NumberOfEmployees = indicator.SelectedValue.Value;
            }
        }
    };
    LeadTestComponent.prototype.isAddress = function (type) {
        return type.Value == 4;
    };
    LeadTestComponent.prototype.isAnnualRevenue = function (type) {
        return type.Value == 2;
    };
    LeadTestComponent.prototype.isEmployeeCount = function (type) {
        return type.Value == 1;
    };
    LeadTestComponent.prototype.getDisplayFor = function (value) {
        return value.Value;
    };
    LeadTestComponent.prototype.getSelectedDisplayValue = function (indicatorType, source) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.SelectedValue != null && indicator.IndicatorType.Value == indicatorType.Value) {
                return this.getDisplayFor(indicator.SelectedValue);
            }
        }
        return "";
    };
    LeadTestComponent.prototype.bindLead = function (leadIndicatorViewModel) {
        this.lead = leadIndicatorViewModel.Result;
    };
    __decorate([
        core_1.ViewChild(AlertList_1.ServerMessageListComponenet), 
        __metadata('design:type', AlertList_1.ServerMessageListComponenet)
    ], LeadTestComponent.prototype, "errorList", void 0);
    LeadTestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'dashboard-component',
            templateUrl: 'leads.html',
            directives: [router_1.ROUTER_DIRECTIVES]
        }), 
        __metadata('design:paramtypes', [serverService_1.ServerService, errorrService_1.ErrorService, channel_service_1.ChannelService])
    ], LeadTestComponent);
    return LeadTestComponent;
}(basePage_1.BasePage));
exports.LeadTestComponent = LeadTestComponent;
//# sourceMappingURL=LeadTestComponent.js.map