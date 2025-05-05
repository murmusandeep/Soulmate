import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { RouterLink } from '@angular/router';
import { TimeagoModule } from 'ngx-timeago';
import { TitleCasePipe } from '@angular/common';
import { Message } from '../../_models/message';

@Component({
  selector: 'app-messages',
  standalone: true,
  imports: [FormsModule, ButtonsModule, PaginationModule, RouterLink, TimeagoModule, TitleCasePipe],
  templateUrl: './messages.component.html',
  styleUrl: './messages.component.scss'
})
export class MessagesComponent implements OnInit {

  messageService = inject(MessageService);
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  isOutbox = this.container === 'Outbox';

  ngOnInit(): void {
    this.getMessages();
  }

  getMessages() {
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container);
  }

  onPageChange(event: any) {
    if (this.pageNumber != event.page) {
      this.pageNumber = event.page;
      this.getMessages();
    }
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => {
        this.messageService.paginatedResult.update((prev) => {
          if (prev && prev.items) {
            prev.items.splice(prev.items.findIndex(m => m.id === id), 1);
            return prev;
          }
          return prev;
        });
      }
    });
  }

  getRoute(message: Message) {
    if(this.container === 'Outbox') {
      return `/members/${message.recipientUsername}`;
    } else{
      return `/members/${message.senderUsername}`;
    }
  }
}
