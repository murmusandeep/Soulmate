import { HttpClient } from '@angular/common/http';
import { Injectable, inject, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/paginatedResult';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  baseUrl = environment.apiUrl;

  private http = inject(HttpClient);
  private accountService = inject(AccountService);

  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();
  userParams = signal<UserParams>(new UserParams(this.accountService.currentUser()));

  resetUserParams() {
    this.userParams.set(new UserParams(this.accountService.currentUser()));
  }

  getMembers() {
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));
    if(response) return setPaginatedResponse(response, this.paginatedResult);
    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);
    params = params.append('minAge', this.userParams().minAge);
    params = params.append('maxAge', this.userParams().maxAge);
    params = params.append('gender', this.userParams().gender);
    params = params.append("orderBy", this.userParams().orderBy);
    return this.http.get<Member[]>(this.baseUrl + 'users', { observe: 'response', params }).subscribe({
      next: response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.memberCache.set(Object.values(this.userParams()).join('-'), response);
      }
    });
  }

  getMember(username: string) {
    const member : Member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.body), [])
    .find((m: Member) => m.username === username);
    if (member) return of(member);
    return this.http.get<Member>(this.baseUrl + 'users/username/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      tap(() => {
        const updatedMemberCache = new Map(this.memberCache);
        for (let [key, paginatedResult] of updatedMemberCache.entries()) {
          const members = paginatedResult.body;
            const index: number = members.findIndex((m: Member) => m.username === member.username);
          if (index !== -1) {
            members[index] = { ...members[index], ...member };
            updatedMemberCache.set(key, { ...paginatedResult, body: members });
          }
        }
        this.memberCache = updatedMemberCache;
      })
    );
  }

  setMainPhoto(photo: Photo) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {}).pipe(
      tap(() => {
        const updatedMemberCache = new Map(this.memberCache);
        for (let [key, paginatedResult] of updatedMemberCache.entries()) {
            const members: Member[] = (paginatedResult.body as Member[]).map((member: Member) => {
            if (member.photos) {
              member.photos.forEach((p: Photo) => p.isMain = false);
              const photoToUpdate: Photo | undefined = member.photos.find((p: Photo) => p.id === photo.id);
              if (photoToUpdate) photoToUpdate.isMain = true;
              member.photoUrl = photo.url;
            }
            return member;
            });
          updatedMemberCache.set(key, { ...paginatedResult, body: members });
        }
        this.memberCache = updatedMemberCache;
      })
    );
  }

  deletePhoto(photo: Photo) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id).pipe(
      tap(() => {
        const updatedMemberCache = new Map(this.memberCache);
        for (let [key, paginatedResult] of updatedMemberCache.entries()) {
            const members: Member[] = (paginatedResult.body as Member[]).map((member: Member) => {
            if (member.photos) {
              member.photos = member.photos.filter((p: Photo) => p.id !== photo.id);
            }
            return member;
            });
          updatedMemberCache.set(key, { ...paginatedResult, body: members });
        }
        this.memberCache = updatedMemberCache;
      })
    );
  }
}
