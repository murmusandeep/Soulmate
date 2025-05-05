import { Component, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { Member } from '../../../_models/member';
import { User } from '../../../_models/user';
import { AccountService } from '../../../_services/account.service';
import { MemberService } from '../../../_services/member.service';
import { MessageService } from 'primeng/api';
import { FormsModule, NgForm } from '@angular/forms';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-member-edit',
  standalone: true,
  imports: [TabsModule, FormsModule, PhotoEditorComponent, TimeagoModule, DatePipe],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.scss'
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  
  member?: Member;

  private accountService = inject(AccountService);
  private memberService = inject(MemberService);
  private messageService = inject(MessageService);


  ngOnInit() {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();
    if (!user) return;
    this.memberService.getMember(user.username).subscribe({
      next: member => {
        this.member = member;
      }
    });
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Member update successfully', life: 2000 }),
      complete: () => this.editForm?.reset(this.member)
    });
  }

  onMemberChange(event: Member) {
    this.member = event;
  }

}
