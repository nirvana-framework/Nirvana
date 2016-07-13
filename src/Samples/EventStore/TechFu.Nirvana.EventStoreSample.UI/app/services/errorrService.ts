import {Injectable, EventEmitter} from '@angular/core';
import {Dictionary} from "../models/common/collections";
import {ValidationMessage} from "../models/CQRS/Commands";
import forEach = require("core-js/fn/array/for-each");


@Injectable()
export class ErrorService {
    private errors:Dictionary<string,EventEmitter<ValidationMessage[]>>;
    private registeredComponents:Array<string> = new Array<string>();

    constructor() {
        this.errors = new Dictionary<string,EventEmitter<ValidationMessage[]>>();
    }



    public registerComponent(componentName:string):EventEmitter<ValidationMessage[]> {
        if (!this.errors.containsKey(componentName)) {
            this.errors.add(componentName, new EventEmitter<ValidationMessage[]>());
        } else {
            console.log('dispose did not handle correctly, WTF mate?');
        }
        return this.errors.getValue(componentName);
    }

    public unregisterComponent(errorComponentName:string) {
        if (!this.errors.containsKey(errorComponentName)) {
            console.log('init failed, WTF mate?');
            return;
        }
        this.errors.remove(errorComponentName);
    }

    showErrors(componentName:string, messages:ValidationMessage[]) {
        if (!this.errors.containsKey(componentName)) {
            for (var i = 0; i < messages.length; i++) {
                console.log(`${messages[i].Key}: ${messages[i].Message}`)
            }
        } else {
            this.errors.getValue(componentName).emit(messages)
        }
    }
}