import {Injectable} from '@angular/core';
import {CommandResponse, Command, QueryResponse, Query} from "../models/CQRS/Common";
import {Headers, Http} from '@angular/http';
import {ServerService} from "./serverService";
import {Serializer} from "./util/serializer";
import {isJsObject} from "@angular/platform-browser-dynamic/src/facade/lang";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class Mediator {

    public commandEndpoint:string = 'https://local-commandapi.mean.software:54406/api';
    public queryEndpoint:string = 'https://local-queryapi.mean.software:54410/api';

    constructor(private http:Http, private security:ServerService, private serializer:Serializer) {    }

    private getCommandUrl<U>(command:Command<U>) {
        var url = `${this.commandEndpoint}/${command.endpointName}`;
        return url;
    }
    private getQueryUrl<U>(query:Command<U>) {
        var url = `${this.queryEndpoint}/${query.endpointName}`;
        return url;
    }
    private getHeaders() {
        let headers = new Headers({'Content-Type': 'application/json'});
        headers.append('Authorization', `Bearer ${this.security.getToken()}`);
        return headers;
    }
    public query<U>(query:Query<U>):Promise<QueryResponse<U>> {
        var url = this.getQueryUrl(query) + '?' + this.objectToParams(query);
        return this.http
            .get(url, {headers: this.getHeaders()})
            .toPromise()
            .then(res=>res.json());
    }

    public command<U>(command:Command<U>):Promise<CommandResponse<U>> {
        return this.http
            .post(this.getCommandUrl(command), this.serializer.serialize(command), {headers: this.getHeaders()})
            .toPromise()
            .then(res=>res.json())
    }


    objectToParams(object):string {
        return Object.keys(object).map((key) =>this.isEndPoint(key) ? '' : isJsObject(object[key]) ?
            this.subObjectToParams(encodeURIComponent(key), object[key])
            : `${encodeURIComponent(key)}=${this.cleanseValue(object, key)}`
        ).join('&');
    }

    private cleanseValue(object, key) {
        if (object[key] == null) {
            return '';
        }
        return encodeURIComponent(object[key]);
    }


    subObjectToParams(key, object):string {

        return Object.keys(object)
            .map((childKey) =>this.isEndPoint(key) ? '' : isJsObject(object[childKey]) ?
                    this.subObjectToParams(`${key}[${encodeURIComponent(childKey)}]`, object[childKey])

                    : `${key}[${encodeURIComponent(childKey)}]=${this.cleanseValue(object, childKey)}`
                // :`${key}[${encodeURIComponent(childKey)}]=${encodeURIComponent(object[childKey])}`
            ).join('&');
    }

    //endpointName and typescriptPlace are used only for communication and TS copatability
    private isEndPoint(key:string) {
        return key == 'endpointName'
        || key=='typescriptPlace';
    }

}
