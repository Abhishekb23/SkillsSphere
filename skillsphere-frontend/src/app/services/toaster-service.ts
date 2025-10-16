import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

export interface ToastMessage {
  type: 'success' | 'error' | 'info';
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class ToasterService {
  private toastSubject = new Subject<ToastMessage>();
  toastState$ = this.toastSubject.asObservable();

  success(message: string) {
    this.toastSubject.next({ type: 'success', message });
  }

  error(message: string) {
    this.toastSubject.next({ type: 'error', message });
  }

  information(message: string) {
    this.toastSubject.next({ type: 'info', message });
  }
}
