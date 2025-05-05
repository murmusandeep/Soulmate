import { Component, inject, input, OnInit, output, ViewChild } from '@angular/core';
import { Message } from '../../../_models/message';
import { MessageService } from '../../../_services/message.service';
import { FormsModule, NgForm } from '@angular/forms';
import { TimeagoModule } from 'ngx-timeago';

@Component({
  selector: 'app-member-message',
  standalone: true,
  imports: [FormsModule, TimeagoModule],
  templateUrl: './member-message.component.html',
  styleUrl: './member-message.component.scss'
})
export class MemberMessageComponent {

  @ViewChild('messageForm') messageForm?: NgForm;

  messageService = inject(MessageService);
  username = input.required<string>();
  messages = input.required<Message[]>();
  updateMessages = output<Message>();
  messageContent = '';

  sendMessage(){
    if (!this.username()) return;
    this.messageService.sendMessage(this.username(), this.messageContent).subscribe({
      next: message => {
        this.updateMessages.emit(message);
        this.messageForm?.resetForm();
      }
    })
  }
}
