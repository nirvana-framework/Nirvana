import { Component} from '@angular/core';
import { MastHead} from '../models/common/masthead';
import {Router} from '@angular/router';
import { ServerService } from '../services/serverService';


@Component({
    moduleId:module.id,
    selector: 'header-component',
    templateUrl:'header.html',
})
export class HeaderComponent {
    constructor( 
        private _securityService:ServerService
        ,private router:Router
    ){}


    masthead: MastHead;
    ngOnInit(){
        this.masthead = this._securityService.getMastHead();
    }

    doLogOut(){
        this._securityService.logOut();
        this.router.navigate(['/']);
    }
    loggedIn(){
        return  this._securityService.getCurrentUser().loggedIn();
    }
}


