import {ValidationMessage} from "./Commands";
import {Subject} from "rxjs/Rx";
export class ServerException{    public Message:string;}
export class PagedResult<T>{public Results:T[] ; public LastPage:number; public Total:number;public PerPage:number;public Page:number}
export abstract class Command<TResult> {    constructor(public endpointName:string){}}
export abstract class Query<TResult> {constructor(public endpointName:string){}}

export abstract class Response {
    public ValidationMessages:ValidationMessage[];
    public IsValid:boolean;
    public Exception:ServerException;

    public Success() {
        return this.IsValid && this.Exception == null;
    }
}

export class CommandResponse<TResult> extends Response {
    public Result: TResult;
    public Throw(){
        if(this.Exception){
            throw this.Exception.Message
        }
    }
}
export class QueryResponse<TResult> extends Response {
    public Result: TResult;
    public Throw(){
        if(this.Exception){
            throw this.Exception.Message
        }
    }
}


export class SignalrWindow extends Window {
    $: any;
}

export enum ConnectionState {
    Connecting = 1,
    Connected = 2,
    Reconnecting = 3,
    Disconnected = 4
}

export class ChannelConfig {
    url: string;
    hubName: string;
    channel: string;
}

export class ChannelEvent {
    Name: string;
    ChannelName: string;
    Timestamp: Date;
    Data: any;
    Json: string;

    constructor() {
        this.Timestamp = new Date();
    }
}
export class ChannelSubject {
    channel: string;
    subject: Subject<ChannelEvent>;
}