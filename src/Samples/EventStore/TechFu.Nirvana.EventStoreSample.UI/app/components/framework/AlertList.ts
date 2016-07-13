import forEach = require("core-js/fn/array/for-each");
import {Component, Input} from "@angular/core";
import {AlertComponent} from "ng2-bootstrap/ng2-bootstrap";
import {ValidationMessage, MessageType} from "../../models/CQRS/Commands";
@Component({
    moduleId: module.id,
    selector: 'server-message-list',
    templateUrl: 'alerts.html',
    directives: [AlertComponent]
})
export class ServerMessageListComponenet {

    @Input() public infoMessages:ValidationMessage[] = [];
    @Input() public warningMessages:ValidationMessage[] = [];
    @Input() public errorMessages:ValidationMessage[] = [];
    @Input() public exceptionMessages:ValidationMessage[] = [];


    public clearAll() {
        this.warningMessages = [];
        this.infoMessages = [];
        this.errorMessages = [];
        this.exceptionMessages = [];
    }

    public setMessages(messages:ValidationMessage[]) {
        for (var i = 0; i < messages.length; i++) {
            let message = messages[i];
            if (message.Key == "")
            {
                switch (message.MessageType) {
                    case MessageType.Error:
                        this.errorMessages.push(message);
                        break;
                    case MessageType.Exception:
                        this.exceptionMessages.push(message);
                        break;
                    case MessageType.Warning:
                        this.warningMessages.push(message);
                        break;
                    case MessageType.Info:
                        this.infoMessages.push(message);
                        break;
                }
            }
        }
    }

}
