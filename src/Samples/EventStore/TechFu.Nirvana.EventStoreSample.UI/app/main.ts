import {bootstrap}    from '@angular/platform-browser-dynamic';
import {HTTP_PROVIDERS} from '@angular/http';
import {APP_ROUTER_PROVIDERS} from "./app.routes";

import {AppComponent} from './app.component';

import "rxjs/add/operator/map";
import {ChannelConfig, ChannelService, SignalrWindow} from "./components/framework/signlar/channel.service";


let channelConfig = new ChannelConfig();
channelConfig.url = "http://localhost:9123/signalr";
channelConfig.hubName = "EventHub";

bootstrap(AppComponent,
    [
        HTTP_PROVIDERS,
        APP_ROUTER_PROVIDERS,
        ChannelService,
        {provide:SignalrWindow, useValue: window,multi:false},
        {provide:"channel.config", useValue: channelConfig,multi:false}
    ]);
