import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikeService } from '../../_services/like.service';
import { Member } from '../../_models/member';
import { FormsModule } from '@angular/forms';
import { MemberCardComponent } from "../members/member-card/member-card.component";
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  standalone: true,
  imports: [FormsModule, ButtonsModule, MemberCardComponent, PaginationModule],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.scss'
})
export class ListsComponent implements OnInit, OnDestroy {

  likeService = inject(LikeService);
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    this.getLikes();
  }

  ngOnDestroy(): void {
    this.likeService.paginatedResult.set(null);
  }

  getTitle() {
    switch (this.predicate) {
      case 'liked':
        return 'People you liked';
      case 'likedBy':
        return 'People who liked you';
      default:
        return 'Mutual';
    }
  }

  getLikes() {
    this.likeService.getLikes(this.predicate, this.pageNumber, this.pageSize);
  }

  onPageChange(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.getLikes();
    }
  }
}
