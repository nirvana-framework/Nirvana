import {Component, OnInit, ViewContainerRef} from '@angular/core';
import {Location} from '@angular/common';
import {ROUTER_DIRECTIVES} from '@angular/router';
import {CookieService} from "angular2-cookie/core";

import {HeaderComponent} from './components/header.component';
import {NavComponent} from './components/nav.component';
import {ServerService} from "./services/serverService";
import {CookieWrapper} from "./services/util/cookieWrapper";
import {ErrorService} from "./services/errorrService";
import 'rxjs/add/operator/toPromise';
import {Observable} from "rxjs/Rx";
import {ChannelService} from "./components/framework/signlar/channel.service";
import {ConnectionState, AppConstants} from "./models/CQRS/Common";


@Component({
    selector: 'my-app',
    moduleId: module.id,
    templateUrl: 'app.html',
    directives: [HeaderComponent, NavComponent, ROUTER_DIRECTIVES],
    providers: [Location, CookieWrapper, ServerService, CookieService, ErrorService,ChannelService,AppConstants],
})
export class AppComponent implements OnInit {

    connectionState$: Observable<string>;
    constructor(private channelService: ChannelService){

    }

    ngOnInit() {
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

        console.log("Starting the channel service");
        this.channelService.start();
    }

}
