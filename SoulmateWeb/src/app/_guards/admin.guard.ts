import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { MessageService } from 'primeng/api';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const messageService = inject(MessageService);
  if (accountService.roles().includes('Admin') || accountService.roles().includes('Moderator')) {
    return true;
  }
  else {
    messageService.add({ severity: 'error', summary: 'Error', detail: 'You are not Authorize', life: 1000 });
    return false;
  }
};
