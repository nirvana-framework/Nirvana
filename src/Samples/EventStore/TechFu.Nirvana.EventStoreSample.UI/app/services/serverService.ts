import {Injectable, EventEmitter} from '@angular/core';
import {Http} from '@angular/http';
import {Router} from '@angular/router';
import 'rxjs/add/operator/toPromise';
import {CookieWrapper} from './util/cookieWrapper';

import {User} from '../models/security/user';
import {StringHelper} from './util/stringHelper';
import {MastHead} from '../models/common/masthead';
import {Mediator} from './apiService';

import {Serializer} from "./util/serializer";
import {CommandResponse, AppConstants, QueryResponse,} from "../models/CQRS/Common";
import {ValidationMessage, MessageType, SessionViewModel} from "../models/CQRS/Commands";
import {ErrorService} from "./errorrService";


@Injectable()
export class ServerService {
    private currentUser:User = new User();
    private masthead:MastHead = new MastHead();
    private serializer:Serializer;
    public mediator:Mediator;
    public authKeys:any = {};

    private onLogin:EventEmitter<boolean> = new EventEmitter<boolean>();
    sessionId:string;
    private session:SessionViewModel;

    constructor(public router:Router, private http:Http, private cookies:CookieWrapper,private errorService:ErrorService,private constants: AppConstants) {
        this.serializer= new Serializer()
        this.mediator = new Mediator(this.http,this,this.serializer);
        this.masthead.setTitle('Nirvana Event Store Sample');
        this.authKeys.loginCookie = "loginCookie";
        this.authKeys.sessionCookie = "sessionCookie";

        //Get this from cookie later...
        this.sessionId =this.getSessionId();
    }

    public setSession(session:QueryResponse<SessionViewModel>){
         this.session= session.Result;
        this.sessionId = this.session.Id;
        var expiration = new Date();
        expiration.setDate(expiration.getDate() + 30);
        this.cookies.setCookie(this.authKeys.sessionCookie,this.session,expiration);
    }


    handleError(componentName:string,error:any) {
        if(error) {
            var errors = [];
            this.errorService.showErrors(componentName, errors);
        }
        else {
            this.errorService.showErrors(componentName, [new ValidationMessage(MessageType.Exception,"","There was a problem logging you in, please try again.")]);
        }

    };

    checkLoggedIn() {
        return this.currentUser.loggedIn();
    }

    getCurrentUser() {
        return this.currentUser;
    }
    getToken() {
        return this.currentUser.token;
    }

    getMastHead() {
        return this.masthead;
    }

    setToken(token) {
        this.currentUser.token = token;
    }

    getLoginEvent() {
        return this.onLogin;
    }


    logOut(){
        this.cookies.removeCookie(this.authKeys.loginCookie);
        this.currentUser.logOut();
        this.onLogin.emit(this.currentUser.loggedIn())
    }


    private getSessionId():string {
        let session = <SessionViewModel>this.cookies.getCookie(this.authKeys.sessionCookie,new SessionViewModel());
       if(session.Id==""){
            return this.constants.EmptyGuid;
        }
       return session.Id;
    }
}