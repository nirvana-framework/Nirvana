import {Component, Input, ViewContainerRef, ViewChild, ElementRef} from '@angular/core';
import {BasePage} from "./basePage";
import {ErrorService} from "../../services/errorrService";
import {PAGINATION_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap'
import {Router, ActivatedRoute} from '@angular/router';
import {ServerMessageListComponenet} from "../framework/AlertList";
import {ServerService} from "../../services/serverService";



@Component({
    moduleId:module.id,
    selector: 'search-component',
    templateUrl: 'search.html',
    directives: [PAGINATION_DIRECTIVES],
})
export class SearchResultsComponent extends BasePage{


    @ViewChild(ServerMessageListComponenet)
    private errorList: ServerMessageListComponenet;

    @Input() public searchTerm:string;
    @Input() public page:number;
    @Input() public itemsPerPage:number;


    constructor(_securityService:ServerService, errorService:ErrorService, private router:Router,route: ActivatedRoute) {
        super(_securityService,errorService,route);
        this.componentName = 'searchResults';
    }
    ngOnInit(){
        this.page=1;
        this.itemsPerPage=20;

        this.registerEvents(this.errorList);
        this.keyParameterSub = this.route.params.subscribe(params => {
            this.searchTerm = params['searchTerm'];
            this.doClientSearch();
        });
    }
    ngOnDestroy(){
        this.disposeRegisteredEvents();
    }

    public pageChanged(event:any):void {
        this.page=event.page;
        this.doClientSearch();
    };


    doClientSearch(){

        console.log("do Search");

    }

  

}


