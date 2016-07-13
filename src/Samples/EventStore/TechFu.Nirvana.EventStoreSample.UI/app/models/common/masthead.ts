import {User} from "../security/user";
export class MastHead {

    public title:string;
    public logoutMessage:string;
    public welcomeMessage:string;

    constructor() {
        this.title = 'Nirvana Event Store Sample'
        this.logoutMessage = 'Log Out';
    }

    setTitle(title) {
        this.title = title;
    }

    setWelcomeMessage(user:User):void {
        this.welcomeMessage = user.userName;
    }
}