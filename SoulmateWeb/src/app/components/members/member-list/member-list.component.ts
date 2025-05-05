import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MemberService } from '../../../_services/member.service';
import { MemberCardComponent } from "../member-card/member-card.component";
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';


@Component({
  selector: 'app-member-list',
  standalone: true,
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule],
  templateUrl: './member-list.component.html',
  styleUrl: './member-list.component.scss'
})
export class MemberListComponent implements OnInit {

  memberService = inject(MemberService);
  genderList = [{ value: 'male', display: 'Male' }, { value: 'female', display: 'Female' }];

  ngOnInit(): void {
    this.memberService.resetUserParams();
    if(!this.memberService.paginatedResult()) this.getMembers();
  }

  getMembers() {
    this.memberService.getMembers();
  }

  onPageChange(event: any) {
    if(this.memberService.userParams().pageNumber !== event.page) {
      this.memberService.userParams().pageNumber = event.page;
      this.getMembers();
    }
  }

  resetFilter() {
    this.memberService.resetUserParams();
    this.getMembers();
  }
}
