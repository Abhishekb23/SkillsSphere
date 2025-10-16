import { Component } from '@angular/core';
import { ToasterService, ToastMessage } from '../../services/toaster-service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-toaster',
  imports: [CommonModule],
  templateUrl: './toaster.html',
  styleUrl: './toaster.css'
})
export class Toaster {
   message = '';
  type: 'success' | 'error' | 'info' = 'info';
  show = false;

  constructor(private toasterService: ToasterService) {
    this.toasterService.toastState$.subscribe((toast: ToastMessage) => {
      this.message = toast.message;
      this.type = toast.type;
      this.show = true;

      setTimeout(() => {
        this.show = false;
      }, 4000);
    });
  }
}
