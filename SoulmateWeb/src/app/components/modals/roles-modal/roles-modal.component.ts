import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.scss'
})
export class RolesModalComponent {

  bsModalRef = inject(BsModalRef);
  title = '';
  username = '';
  availableRoles: any[] = [];
  selectedRoles: any[] = [];
  rolesUpdated = false

  updateChecked(checkedValue: string) {
    if(this.selectedRoles.includes(checkedValue)) {
      this.selectedRoles = this.selectedRoles.filter(r => r != checkedValue)
    } else {
      this.selectedRoles.push(checkedValue);
    }
  }

  onSelectRoles() {
    this.rolesUpdated = true;
    this.bsModalRef.hide();
  }

}
