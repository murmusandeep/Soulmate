import { inject, Injectable, signal } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { MessageService } from 'primeng/api';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {

  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private messageService = inject(MessageService);
  private router = inject(Router);
  onlineUsers = signal<string[]>([]);

    createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'presence', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch(error => console.log(error))

    this.hubConnection.on('UserIsOnline', username => {
      this.onlineUsers.update(users => [...users, username]);
    });

    this.hubConnection.on('UserIsOffline', username => {
      this.onlineUsers.update(users => users.filter(u => u !== username));
    });

    this.hubConnection.on('GetOnlineUsers', usernames => {
      this.onlineUsers.set(usernames);
    });

    this.hubConnection.on('NewMessageRecieved', ({username, knownAs}) => {
      this.messageService.add({ severity: 'info', summary: 'New Message', detail: `${knownAs} sent you a new message`})
    });
  }

  stopHubConnection() {
    if(this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection?.stop().catch(error => {
        console.log(error)
      });
    }
  }

  navigateBy(username: string) {
    this.router.navigateByUrl('/members/' + username + '?tab=Messages')
  }

}
