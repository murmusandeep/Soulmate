import { Component, inject } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { ImportsModule } from '../../_shared/imports';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { MemberService } from '../../_services/member.service';

@Component({
  selector: 'app-nav',
  standalone: true,
  imports: [ImportsModule, RouterLink, RouterLinkActive, BsDropdownModule],
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent {

  accountService = inject(AccountService);
  memberService = inject(MemberService);
  private router = inject(Router);

  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.memberService.paginatedResult.set(null);
        this.router.navigateByUrl('/members');
      }
    })
  }

  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
