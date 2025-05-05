import { Component, computed, inject, model, OnInit, signal, ViewChild } from '@angular/core';
import { MemberService } from '../../../_services/member.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../../_models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';
import { MemberMessageComponent } from "../member-message/member-message.component";
import { Message } from '../../../_models/message';
import { MessageService } from '../../../_services/message.service';

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
export class MemberDetailComponent implements OnInit {
  
  private memberService = inject(MemberService);
  private route = inject(ActivatedRoute);
  messageService = inject(MessageService);
  
  @ViewChild('memberTabs' , {static: true}) memberTabs?: TabsetComponent;
  activeTab?: TabDirective;
  messages: Message[] = [];
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  
  
  ngOnInit() {
    this.route.data.subscribe({
      next: data => {
        this.member = data['member'];
        //this.getImages();
        this.member && this.member.photos.map(p => {
          this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
        })
      }
    });

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    });
  }

  onUpdateMessages(event: Message) {
    this.messages.push(event);
  }

  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get('username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: member => {
  //       this.member = member;
  //       this.getImages();
  //     }
  //   });
  // }

  // getImages() {
  //   if (!this.member) return;
  //   for (const photo of this.member.photos) {
  //     this.images.push(new ImageItem({ src: photo.url, thumb: photo.url }));
  //   }
  // }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    if (this.activeTab.heading == 'Messages' && this.messages.length === 0 && this.member) {
      this.messageService.getMessageThread(this.member.username).subscribe({
        next: (messages) => {
          this.messages = messages;
        }
      });
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
}
