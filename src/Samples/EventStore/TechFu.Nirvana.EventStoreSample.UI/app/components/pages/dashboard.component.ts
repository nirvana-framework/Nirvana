import {Component, Input, ViewChild} from '@angular/core';
import {ServerService} from '../../services/serverService';
import forEach = require("core-js/fn/array/for-each");
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {TestCommand} from "../../models/CQRS/Commands";
import {ROUTER_DIRECTIVES} from "@angular/router";
import {TaskComponent} from "./TaskComponent";
import {Observable} from "rxjs/Rx";
import {ChannelService} from "../framework/signlar/channel.service";
import {ConnectionState} from "../../models/CQRS/Common";


class StatusEvent {
    State: string;
    PercentComplete: number;
}


@Component({
    moduleId:module.id,
    selector: 'dashboard-component',
    templateUrl: 'dashboard.html',
    directives: [ROUTER_DIRECTIVES,TaskComponent]
})
export class DashboardComponent extends BasePage{

    connectionState$: Observable<string>;

    @ViewChild(ServerMessageListComponenet)
    private errorList: ServerMessageListComponenet;
    private lastClickTime:Date = null;

    constructor(_securityService:ServerService, errorService:ErrorService, private channelService: ChannelService) {
        super(_securityService,errorService,null);
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);


        // Let's wire up to the signalr observables
        //
        this.connectionState$ = this.channelService.connectionState$
            .map((state: ConnectionState) => { return ConnectionState[state]; });

        this.channelService.error$.subscribe(
            (error: any) => { console.warn(error); },
            (error: any) => { console.error("errors$ error", error); }
        );

        // Wire up a handler for the starting$ observable to log the
        //  success/fail result
        //
        this.channelService.starting$.subscribe(
            () => { console.log("signalr service has been started"); },
            () => { console.warn("signalr service failed to start!"); }
        );

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
       this.lastClickTime = new Date();
    }

}
