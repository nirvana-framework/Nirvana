import {Component, OnInit} from '@angular/core';
import {NgFor} from '@angular/common';
import {ServerService} from '../services/serverService';
import {ROUTER_DIRECTIVES, Router,ActivatedRoute } from '@angular/router';
@Component({
    moduleId: module.id,
    selector: 'nav-component',
    templateUrl: 'nav.html',
    directives: [ROUTER_DIRECTIVES, NgFor],
})

export class NavComponent implements OnInit {
    loggedIn:boolean;
    links:NavItem[];
    searchTerm:string;

    constructor(private _securityService:ServerService, private router:Router, private activeRoute:ActivatedRoute) {
    }

    ngOnInit() {
        this.searchTerm = '';
        this.loggedIn = this._securityService.checkLoggedIn();
        this._securityService.getLoginEvent().subscribe(x=>this.loggedIn = x);
        this.links = [

        ]
    }

   

    doSearch() {
        this.router.navigate(['/searchResults', this.searchTerm]);
    }

}


export class NavItem {
    constructor(public route:string, public  text:string) {

    }

}
