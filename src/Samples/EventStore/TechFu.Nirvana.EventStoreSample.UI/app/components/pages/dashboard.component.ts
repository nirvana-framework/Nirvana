import {Component, Input, ViewChild} from '@angular/core';
import {ServerService} from '../../services/serverService';
import forEach = require("core-js/fn/array/for-each");
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {TestCommand} from "../../models/CQRS/Commands";
import {ROUTER_DIRECTIVES} from "@angular/router";
import {Observable} from "rxjs/Rx";
import {ChannelService} from "../framework/signlar/channel.service";
import {ConnectionState, ChannelEvent} from "../../models/CQRS/Common";


class StatusEvent {
    State: string;
    PercentComplete: number;
}


@Component({
    moduleId:module.id,
    selector: 'dashboard-component',
    templateUrl: 'dashboard.html',
    directives: [ROUTER_DIRECTIVES]
})
export class DashboardComponent extends BasePage{

    private receivedMessage:string;
    private sentMessage:string;
    private channel = "tasks";

    connectionState$: Observable<string>;

    @ViewChild(ServerMessageListComponenet)
    private errorList: ServerMessageListComponenet;

    constructor(_securityService:ServerService, errorService:ErrorService, private channelService: ChannelService) {
        super(_securityService,errorService,null);
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
        this.sentMessage='';
        this.receivedMessage='';

        this.connectionState$ = this.channelService.connectionState$
            .map((state: ConnectionState) => { return ConnectionState[state]; });

        this.channelService.error$.subscribe(
            (error: any) => { console.warn(error); },
            (error: any) => { console.error("errors$ error", error); }
        );

        this.channelService.starting$.subscribe(
            () => { console.log("signalr service has been started"); },
            () => { console.warn("signalr service failed to start!"); }
        );


        this.channelService.sub(this.channel).subscribe(
            (x:ChannelEvent) => {
                switch (x.Name) {
                    case 'Infrastructure::TestUiEvent': {
                        this.appendStatusUpdate(x);
                    }
                }
            },
            (error:any) => {
                console.warn("Attempt to join channel failed!", error);
            }
        )

    }
    ngOnInit(){
        // Start the connection up!
        //
        console.log("Starting the channel service");

        this.channelService.start();
    }
    ngOnDestroy(){
        this.disposeRegisteredEvents();
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
