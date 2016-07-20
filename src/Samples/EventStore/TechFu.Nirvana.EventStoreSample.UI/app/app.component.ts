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


@Component({
    selector: 'my-app',
    moduleId: module.id,
    templateUrl: 'app.html',
    directives: [HeaderComponent, NavComponent, ROUTER_DIRECTIVES],
    providers: [Location, CookieWrapper, ServerService, CookieService, ErrorService,ChannelService],
})
export class AppComponent implements OnInit {


    ngOnInit() {

    }

}
