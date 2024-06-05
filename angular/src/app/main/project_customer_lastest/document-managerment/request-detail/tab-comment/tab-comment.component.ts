import { EsignCommentsServiceProxy } from './../../../../../../shared/service-proxies/service-proxies';
import { Component, EventEmitter, Injector, Input, OnInit, Output, ViewChild } from '@angular/core';
import { DateTimeService } from '@app/shared/common/timing/date-time.service';
import { AppComponentBase } from '@shared/common/app-component-base';
import { CreateOrEditEsignCommentsInputDto } from '@shared/service-proxies/service-proxies';
import { ListViewComponent } from '@syncfusion/ej2-angular-lists';
import { trim } from 'lodash-es';
import { finalize } from 'rxjs';

@Component({
    selector: 'tab-comment',
    templateUrl: './tab-comment.component.html',
    styleUrls: ['./tab-comment.component.less']
})
export class TabCommentComponent extends AppComponentBase {
    @ViewChild('listviewComment') listviewComment : ListViewComponent;
    @Input() comments: any;
    @Input() requestId: number;
    @Input() statusCode: string;
    @Input() allowAction: boolean = true;
    @Output() addComment: EventEmitter<any> = new EventEmitter<any>();
    messageInput;
    saving = false;
    textplaceHoder = 'Write a comment...';

    constructor(
        injector: Injector,
        private dateFormat: DateTimeService,
        private readonly _commentService: EsignCommentsServiceProxy

    ) {
        super(injector);
    }

    ngOnInit() {
    }

    submitComment(){
        this.saving = true;
        trim(this.messageInput)
        if(this.messageInput){
            let comment : CreateOrEditEsignCommentsInputDto = new CreateOrEditEsignCommentsInputDto ({
                requestId: this.requestId,
                content: this.messageInput,
                isPublic: true,
                id: 0
            })
            this._commentService.createOrEditEsignComments(
                comment
            )
            .pipe(finalize(() => {
                this.saving = false;
            }))
            .subscribe(() => {
                this.addComment.emit(null);
                this.messageInput = '';
            })
        }
        else{
            this.saving = false;
            return;
        }
    }
    formatDate(input){
        if(input){
            //check in day
            let date = new Date(input);
            let now = new Date();
            if(date.getDate() === now.getDate() && date.getMonth() === now.getMonth() && date.getFullYear() === now.getFullYear()){
                //return 10 minutes ago
                let minutes = Math.floor((now.getTime() - date.getTime()) / 60000);
                if(minutes <= 0){
                    return 'Just now';
                }
                else if(minutes < 60){
                    return minutes + 'm ago';
                }else{
                    let hours = Math.floor(minutes / 60);
                    return hours + 'h ago';
                }
            }
            //in year return type Oct 21
            else if(date.getFullYear() === now.getFullYear()){
                return this.dateFormat.formatDate(date, 'MMM d, hh:mm a');
            }
            else{
                //return type Oct 21, 2020
                return this.dateFormat.formatDate(input as Date, 'MMM d, yyyy');
            }
        }
        else return '';
    }
    changeKey(event){
        if(event.keyCode === 13 && this.checkInput(this.messageInput)){
            this.submitComment();
        }
    }

    checkInput(input){
        if(!trim(input) || trim(input).includes('\n')){
            return false;
        }
        else
        return true;
    }
}
