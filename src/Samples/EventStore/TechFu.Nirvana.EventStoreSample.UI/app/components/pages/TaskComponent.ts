import {Component, OnInit, Input} from "@angular/core";
import {Http, Response} from "@angular/http";
import {ChannelService} from "../framework/signlar/channel.service";
import {ChannelEvent} from "../../models/CQRS/Common";


class StatusEvent {
    State:string;
    PercentComplete:number;
}

@Component({
    moduleId: module.id,
    selector: 'task',
    templateUrl: 'TaskComponent.html'
})
export class TaskComponent implements OnInit {
    @Input() eventName:string;
    @Input() apiUrl:string;

    messages = "";

    private channel = "tasks";

    constructor(private http:Http,private channelService:ChannelService) {

    }

    ngOnInit() {

        this.channelService.sub(this.channel).subscribe(
            (x:ChannelEvent) => {
                switch (x.Name) {
                    case 'Infrastructure::TestUiEvent': {
                        this.appendStatusUpdate(x);
                    }
                }
            },
            (error:any) => {
                console.warn("Attempt to join channel failed!", error);
            }
        )
    }


    private appendStatusUpdate(ev:ChannelEvent):void {
        this.messages += `${new Date().toLocaleTimeString()} : ` + JSON.stringify(ev.Data);
    }

}
