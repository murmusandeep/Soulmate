import { Component, computed, inject, model, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../../_services/member.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Member } from '../../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessageComponent } from "../member-message/member-message.component";
import { Message } from '../../../_models/message';
import { MessageService } from '../../../_services/message.service';
import { PresenceService } from '../../../_services/presence.service';
import { AccountService } from '../../../_services/account.service';
import { HubConnectionState } from '@microsoft/signalr';

export interface GalleriaImage {
  itemImageSrc: string;
  thumbnailImageSrc: string;
  alt?: string;
  title?: string;
}

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [TabsModule, GalleryModule, TimeagoModule, DatePipe, MemberMessageComponent],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.scss'
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  messageService = inject(MessageService);
  presenceService = inject(PresenceService);
  private accountService = inject(AccountService);
  
  @ViewChild('memberTabs' , {static: true}) memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  
  
  ngOnInit() {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        this.member && this.member.photos.map(p => {
          this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
        })
      }
    });

    this.route.queryParams.subscribe({
      next: _ => this.onRouteParamsChange()
    });

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    });
  }

  onRouteParamsChange(){
    const user = this.accountService.currentUser();
    if(!user) return;
    if(this.messageService.hubConnection?.state === HubConnectionState.Connected && this.activeTab?.heading == 'Messages'){
      this.messageService.hubConnection.stop().then(() => {
        this.messageService.createHubConnection(user, this.member.username);
      });
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tab: this.activeTab.heading },
      queryParamsHandling: 'merge'
    });
    if (this.activeTab.heading == 'Messages' && this.member) {
      const user = this.accountService.currentUser();
      if(!user) return;
      this.messageService.createHubConnection(user, this.member.username);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const messageTab =  this.memberTabs.tabs.find(x => x.heading == heading);
      if(messageTab) {
        messageTab.active = true;
      }
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
