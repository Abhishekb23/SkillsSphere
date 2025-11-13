import { Component, NgZone } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToasterService, ToastMessage } from '../../services/toaster.service';

@Component({
  selector: 'app-toaster',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toaster.html',
  styleUrl: './toaster.css'
})
export class Toaster {

  message = '';
  type: 'success' | 'error' | 'info' = 'info';
  show = false;

  private hideTimeout: any;

  constructor(
    private toasterService: ToasterService,
    private ngZone: NgZone
  ) {

    this.toasterService.toastState$.subscribe((toast: ToastMessage) => {

      // Clear previous timeout so new toast is not affected
      if (this.hideTimeout) {
        clearTimeout(this.hideTimeout);
      }

      this.message = toast.message;
      this.type = toast.type;
      this.show = true;

      // Run timer outside Angular for performance
      this.ngZone.runOutsideAngular(() => {
        this.hideTimeout = setTimeout(() => {
          this.ngZone.run(() => (this.show = false));
        }, 3000); // <-- ALWAYS show for 3 seconds
      });
    });
  }
}
