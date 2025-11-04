import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../services/loading-bar-service';

@Component({
  selector: 'app-loading-bar',
  standalone: true,
  imports: [CommonModule],
  template: `<div class="loading-bar" [class.active]="loading"></div>`,
  styles: [`
    .loading-bar {
      position: fixed;
      top: 0;
      left: 0;
      height: 3px;
      width: 0%;
      background-color: #2196f3;
      z-index: 9999;
      transition: width 0.3s ease, opacity 0.3s ease;
      opacity: 0;
    }
    .loading-bar.active {
      width: 100%;
      opacity: 1;
    }
  `]
})
export class LoadingBarComponent {
  loading = false;

  constructor(private loadingService: LoadingService) {
    this.loadingService.loading$.subscribe(status => this.loading = status);
  }
}
