import { bootstrap }    from '@angular/platform-browser-dynamic';
import {AppComponent} from './app.component';
import { HTTP_PROVIDERS } from '@angular/http';
import {APP_ROUTER_PROVIDERS} from "./app.routes";

// enableProdMode();
bootstrap(AppComponent, [HTTP_PROVIDERS,APP_ROUTER_PROVIDERS]);
