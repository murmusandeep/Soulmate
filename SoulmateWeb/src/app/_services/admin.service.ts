import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { User } from '../_models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  baseUrl = environment.apiUrl;
  private http = inject(HttpClient);

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUsersRole(username: string, roles: string[]) {
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, {});
  }
}
