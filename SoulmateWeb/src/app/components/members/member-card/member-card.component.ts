import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../../_models/member';
import { RouterLink } from '@angular/router';
import { LikeService } from '../../../_services/like.service';
import { MessageService } from 'primeng/api';
import { PresenceService } from '../../../_services/presence.service';

@Component({
  selector: 'app-member-card',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.scss'
})
export class MemberCardComponent {
  private likeService = inject(LikeService);
  private messageService = inject(MessageService);
  private presenceService = inject(PresenceService);
  member = input.required<Member>();
  hasLiked = computed(() => this.likeService.likeIds().includes(this.member().id));
  isOnline = computed(() => this.presenceService.onlineUsers().includes(this.member().username));

  toggleLike() {
    const memberId = this.member().id;
    const knownAs = this.member().knownAs;
    const alreadyLiked = this.hasLiked();
  
    this.likeService.toggleLike(memberId).subscribe({
      next: () => {
        this.likeService.likeIds.update(ids => alreadyLiked ? ids.filter(id => id !== memberId) : [...ids, memberId]);

        const action = alreadyLiked ? 'unliked' : 'liked';
        this.messageService.add({ severity: 'info', summary: 'Info', detail: `You have ${action} ${knownAs}`, life: 2000 });
      }
    });
  }
}
