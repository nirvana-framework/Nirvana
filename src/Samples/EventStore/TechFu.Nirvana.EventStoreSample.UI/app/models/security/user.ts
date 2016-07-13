
export class User {

    public userName:string = '';
    public token:string = '';
    private scope:string;
    private tokenType:string;
    private expires:number;

    constructor() {
        this.userName = '';
        this.token = '';

    }


    loggedIn() {
        return this.token != '';
    };
  
    logOut(){
        this.token = '';
        this.userName ='';
        this.scope = '';
    }

}