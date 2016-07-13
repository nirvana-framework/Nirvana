import {Mediator} from "../../services/apiService";
import {Serializer} from "../../services/util/serializer";
import {ServerService} from "../../services/serverService";
import {ErrorService} from "../../services/errorrService";
import {ValidationMessage, MessageType} from "../../models/CQRS/Commands";
import {Input, ViewChild} from "@angular/core";
import {ServerMessageListComponenet} from "../framework/AlertList";
import {EventEmitter} from "@angular/common/src/facade/async";
import {ActivatedRoute} from "@angular/router";
export abstract class BasePage {

    @Input() public loggedIn;
    @Input() public  componentName:string;
    @Input() protected generalMessages:ValidationMessage[];
    @Input() private validationMessageSubscription;
    @Input() protected onMessagesReceoved:EventEmitter<ValidationMessage[]>  = new EventEmitter<ValidationMessage[]>();



    public mediator:Mediator;
    public serializer:Serializer = new Serializer();
    protected keyParameterSub:any;

    constructor(protected  _serverService:ServerService, protected errors:ErrorService,protected route:ActivatedRoute) {
        this.mediator = this._serverService.mediator;
        this.generalMessages=[];
    }

    receiveErrors(messages:ValidationMessage[]){
        this.generalMessages = messages;
        this.onMessagesReceoved.emit(this.generalMessages)
    }
    protected  registerEvents(alertComponent:ServerMessageListComponenet){
        let emitter = this.errors.registerComponent(this.componentName);
        this.validationMessageSubscription = emitter.subscribe(x=>this.receiveErrors(x))
        this.onMessagesReceoved.subscribe(x=>this.updateAlertComponent(alertComponent,x));
    }
    protected disposeRegisteredEvents() {
        this.errors.unregisterComponent(this.componentName)
        this.onMessagesReceoved.unsubscribe();
        if(this.keyParameterSub) {
            this.keyParameterSub.unsubscribe();
        }
    }


    public showSuccess() {
        this.showInfo("Your Changes were saved");
    }

    public showInfo(message:string){
        this.errors.showErrors(this.componentName,[new ValidationMessage(MessageType.Info,"",message)]);
    }
    public showWarning(message:string){
        this.errors.showErrors(this.componentName,[new ValidationMessage(MessageType.Warning,"",message)]);
    }
    public showError(message:string){
        this.errors.showErrors(this.componentName,[new ValidationMessage(MessageType.Error,"",message)]);
    }
    public showException(message:string){
        this.errors.showErrors(this.componentName,[new ValidationMessage(MessageType.Exception,"",message)]);
    }

    private updateAlertComponent(alertComponent:ServerMessageListComponenet, x:ValidationMessage[]) {
        alertComponent.clearAll();
        alertComponent.setMessages(x);

    }
}
