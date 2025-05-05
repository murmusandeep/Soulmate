import { Component, inject, input, OnInit, output } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { FileUploadModule } from 'ng2-file-upload';
import { Member } from '../../../_models/member';
import { Photo } from '../../../_models/photo';
import { CommonModule } from '@angular/common';
import { User } from '../../../_models/user';
import { environment } from '../../../../environments/environment';
import { MemberService } from '../../../_services/member.service';
import { AccountService } from '../../../_services/account.service';

@Component({
  selector: 'app-photo-editor',
  standalone: true,
  imports: [CommonModule, FileUploadModule],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.scss'
})
export class PhotoEditorComponent implements OnInit {
  
  member = input.required<Member>();
  memberChange = output<Member>();

  uploader: FileUploader | undefined;
  hasBaseDropZoneOver: boolean = false;
  baseUrl = environment.apiUrl;

  private memberService = inject(MemberService);
  private accountService = inject(AccountService);

  ngOnInit() {
    this.initializeUpload();
  }

  setMainPhoto(photo: Photo) {
    this.memberService.setMainPhoto(photo).subscribe({
      next: () => {
        const user = this.accountService.currentUser();
        if(user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        const updatedMember = {...this.member()};
        updatedMember.photoUrl = photo.url;
        updatedMember.photos.forEach(p => {
          if(p.isMain) p.isMain = false;
          if(p.id == photo.id) p.isMain = true;
        })
        this.memberChange.emit(updatedMember);
      }
    });
  }

  deletePhoto(photo: Photo) {
    this.memberService.deletePhoto(photo).subscribe({
      next: () => {
        const updatedMember = {...this.member()};
        updatedMember.photos = updatedMember.photos.filter(x => x.id != photo.id);
        this.memberChange.emit(updatedMember);
      }
    });
  }

  initializeUpload() {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'users/add-photo',
      authToken: 'Bearer ' + this.accountService.currentUser()?.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        const updatedMember = {...this.member()};
        updatedMember.photos.push(photo);
        this.memberChange.emit(updatedMember);

        if(photo.isMain) {
          const user = this.accountService.currentUser();
          if(user) {
            user.photoUrl = photo.url;
            this.accountService.setCurrentUser(user);
          }
          updatedMember.photoUrl = photo.url;
          updatedMember.photos.forEach(p => {
            if(p.isMain) p.isMain = false;
            if(p.id == photo.id) p.isMain = true;
          })
          this.memberChange.emit(updatedMember);
        }
      }
    }
  }

  public fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }
}
