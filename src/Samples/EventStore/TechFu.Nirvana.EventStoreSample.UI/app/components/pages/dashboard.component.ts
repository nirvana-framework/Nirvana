import {Component, Input, ViewChild, OnInit} from '@angular/core';
import {ServerService} from '../../services/serverService';
import forEach = require("core-js/fn/array/for-each");
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {
    TestCommand, CreateSampleCatalogCommand, GetHomepageCataglogViewModelQuery,
    HomePageViewModel
} from "../../models/CQRS/Commands";
import {ROUTER_DIRECTIVES} from "@angular/router";
import {ChannelService} from "../framework/signlar/channel.service";
import {ChannelEvent, QueryResponse} from "../../models/CQRS/Common";



@Component({
    moduleId:module.id,
    selector: 'dashboard-component',
    templateUrl: 'dashboard.html',
    directives: [ROUTER_DIRECTIVES]
})
export class DashboardComponent extends BasePage implements OnInit{

    private receivedMessage:string;
    private sentMessage:string;
    private channel = "tasks";

    public model:HomePageViewModel ;

    @ViewChild(ServerMessageListComponenet)
    private errorList: ServerMessageListComponenet;

    constructor(_securityService:ServerService, errorService:ErrorService, private channelService: ChannelService) {
        super(_securityService,errorService,null);
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
        this.sentMessage='';
        this.receivedMessage='';


        this.channelService.sub(this.channel).subscribe(
            (x:ChannelEvent) => {
                switch (x.Name) {
                    case 'ProductCatalog::HomePageUpdatedUiEvent': {
                        this.refreshHomepageView();
                    }
                }
            },
            (error:any) => {
                console.warn("Attempt to join channel failed!", error);
            }
        )

    }
    refreshHomepageView(){

        this._serverService.mediator.query(new GetHomepageCataglogViewModelQuery()).then(x=>this.setHomePageModel(<QueryResponse<HomePageViewModel>>x))
    }

    ngOnInit(){
        this.refreshHomepageView();
    }

    setHomePageModel(model: QueryResponse<HomePageViewModel> ) {

        this.model  = model.Result;
    }
    ngOnDestroy(){
        this.disposeRegisteredEvents();
    }



    public createTestCatalog(){
        this._serverService.mediator.command(new CreateSampleCatalogCommand());
    }

    public sendCommand(){
        this._serverService.mediator.command(new TestCommand())
            .then(x=>this.showClicked());
    }

    private showClicked() {
        this.sentMessage += new Date().toDateString() + '\n\n';
    }


    private appendStatusUpdate(ev:ChannelEvent):void {
        this.receivedMessage += `${new Date().toLocaleTimeString()} : ` + JSON.stringify(ev.Data)+ '\n\n';
    }

    private clear(){
        this.sentMessage='';
        this.receivedMessage='';
    }

}
