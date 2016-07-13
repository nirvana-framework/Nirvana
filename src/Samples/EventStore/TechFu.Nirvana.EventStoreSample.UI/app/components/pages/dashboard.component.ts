import {Component, Input, ViewChild} from '@angular/core';
import {ServerService} from '../../services/serverService';
import forEach = require("core-js/fn/array/for-each");
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {TestCommand} from "../../models/CQRS/Commands";
@Component({
    moduleId:module.id,
    selector: 'dashboard-component',
    templateUrl: 'dashboard.html',
})
export class DashboardComponent extends BasePage{

    @ViewChild(ServerMessageListComponenet)
    private errorList: ServerMessageListComponenet;
    private lastClickTime:Date = null;

    constructor(_securityService:ServerService, errorService:ErrorService) {
        super(_securityService,errorService,null);
        this.componentName = 'dashboard';
        this.registerEvents(this.errorList);
    }
    ngOnInit(){

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
