import {Injectable} from '@angular/core';
import {CookieService,CookieOptionsArgs} from "angular2-cookie/core";
import {Serializer} from "./serializer";

@Injectable()
export class CookieWrapper{
    // private cookies:CookieService;
    public serializer:Serializer = new Serializer();
    constructor(private cookies:CookieService){
        // var options:CookieOptions = null;
        // this.cookies = new CookieService(options);
    }
    setCookie<T>(key:string,value:T,expires:Date):void{

        let options: CookieOptionsArgs = <CookieOptionsArgs> {
            expires: expires
        };
        var valueString = this.serializer.serialize(value);
        this.cookies.put(key,valueString,options);
    }
    getCookie<T>(key:string,blank:T ):T{
        var valueString = this.cookies.get(key);
        if(!valueString){
            return null;
        }
        var obj :T= this.serializer.deserialize<T>(blank, valueString);
        return obj;
    }
    removeCookie(key:string):void{
        this.cookies.remove(key);
    }
}
