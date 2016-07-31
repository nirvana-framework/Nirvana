import {Component, Input, ViewChild} from '@angular/core';
import {ServerService} from '../../services/serverService';
import forEach = require("core-js/fn/array/for-each");
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {
    TestCommand, CreateSampleCatalogCommand, GetLeadIndicatorOveriewQuery,
    LeadIndicatorViewModel, IndicatorSource, IndicatorType, IndicatorValueViewModel, PerformanceIndicatorViewModel
} from "../../models/CQRS/Commands";
import {ROUTER_DIRECTIVES} from "@angular/router";
import {ChannelService} from "../framework/signlar/channel.service";
import {ChannelEvent, QueryResponse} from "../../models/CQRS/Common";


@Component({
    moduleId: module.id,
    selector: 'dashboard-component',
    templateUrl: 'leads.html',
    directives: [ROUTER_DIRECTIVES]
})
export class LeadTestComponent extends BasePage {

    private receivedMessage:string;
    private sentMessage:string;
    private channel = "tasks";

    public lead:LeadIndicatorViewModel = new LeadIndicatorViewModel();

    @ViewChild(ServerMessageListComponenet)
    private errorList:ServerMessageListComponenet;

    constructor(_securityService:ServerService, errorService:ErrorService, private channelService:ChannelService) {
        super(_securityService, errorService, null);
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
        this.sentMessage = '';
        this.receivedMessage = '';
    }

    ngOnInit() {
        this._serverService.mediator.query(new GetLeadIndicatorOveriewQuery("", true))
            .then(x=>this.bindLead(<QueryResponse<LeadIndicatorViewModel>>x));
    }

    ngOnDestroy() {
        this.disposeRegisteredEvents();
    }

    public getSourceValue(type:IndicatorType, source:IndicatorSource) {
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
    }

    public isSelectedSource(indicatorType:IndicatorType,source:IndicatorSource) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if(indicator.SelectedValue != null && indicator.IndicatorType.Value == indicatorType.Value && indicator.SelectedValue.Source.Value==source.Value){
                return true;
            }
        }
        return false;
    }


    public setSelectedValue(indicatorType:IndicatorType,source:IndicatorSource) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if(indicator.IndicatorType.Value!=indicatorType.Value){
                continue;
            }
            for(var j=0; j<indicator.AllValues.length;j++)
            if (indicator.AllValues[j].Source.Value==source.Value) {
                indicator.SelectedValue = indicator.AllValues[j];
            }
        }
        this.updateBusinessMeasures();
    }
    public updateBusinessMeasures(){
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if(indicator.SelectedValue==null){
                continue;
            }

            if(this.isAddress(indicator.IndicatorType)){
                this.lead.Address=indicator.SelectedValue.Value;
            }

            if(this.isAnnualRevenue(indicator.IndicatorType)){
                this.lead.BusinessMeasure.AnnualRevenue=indicator.SelectedValue.Value;
            }
            if(this.isEmployeeCount(indicator.IndicatorType)){
                this.lead.BusinessMeasure.NumberOfEmployees=indicator.SelectedValue.Value;
            }

        }
    }
    public isAddress(type:IndicatorType){

        return type.Value==4;
    }
    public isAnnualRevenue(type:IndicatorType){

        return type.Value==2;
    }
    public isEmployeeCount(type:IndicatorType){

        return type.Value==1;
    }

    public getDisplayFor(value:IndicatorValueViewModel) {
        return value.Value;
    }

    public getSelectedDisplayValue(indicatorType:IndicatorType,source:IndicatorSource) {
        for (var i = 0; i < this.lead.Indicators.length; i++) {
            var indicator = this.lead.Indicators[i];
            if (indicator.SelectedValue != null && indicator.IndicatorType.Value == indicatorType.Value ) {
                return this.getDisplayFor(indicator.SelectedValue)
            }
        }
        return "";
    }


    private bindLead(leadIndicatorViewModel:QueryResponse<LeadIndicatorViewModel>) {
        this.lead = leadIndicatorViewModel.Result;
    }
}
