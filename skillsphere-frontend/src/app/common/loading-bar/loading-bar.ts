import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingService } from '../../services/loading-bar-service';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-loading-bar',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="loading-bar-wrapper" [class.active]="loading">
      <div class="loading-bar" [style.width.%]="progress"></div>
    </div>
  `,
  styles: [`
    /* --- Wrapper --- */
    .loading-bar-wrapper {
      position: fixed;
      top: 0;
      left: 0;
      height: 4px;
      width: 100%;
      overflow: hidden;
      opacity: 0;
      background: transparent;
      transition: opacity 0.3s ease;
      z-index: 9999;
    }

    .loading-bar-wrapper.active {
      opacity: 1;
    }

    /* --- Animated Gradient Bar --- */
    .loading-bar {
      height: 100%;
      width: 0%;
      border-radius: 0 2px 2px 0;
      background: linear-gradient(
        90deg,
        #3b82f6,
        #2563eb,
        #1d4ed8,
        #3b82f6
      );
      background-size: 300% 100%;
      animation: shimmer 1.5s linear infinite;
      box-shadow: 0 0 8px rgba(37, 99, 235, 0.3);
      transition: width 0.25s ease-out;
    }

    @keyframes shimmer {
      0% {
        background-position: 200% 0;
      }
      100% {
        background-position: -200% 0;
      }
    }

    /* --- Fade Out Smoothly --- */
    .loading-bar-wrapper:not(.active) .loading-bar {
      transition: width 0.25s ease-in, opacity 0.4s ease;
      opacity: 0;
    }
  `]
})
export class LoadingBarComponent implements OnDestroy {
  loading = false;
  progress = 0;
  private timer?: Subscription;

  constructor(private loadingService: LoadingService) {
    this.loadingService.loading$.subscribe(status => {
      this.loading = status;
      if (status) this.startProgress();
      else this.finishProgress();
    });
  }

  private startProgress() {
    this.progress = 0;
    this.timer?.unsubscribe();
    this.timer = interval(180).subscribe(() => {
      // Grow progressively but slow near the end
      if (this.progress < 90) {
        this.progress += Math.random() * 8;
        if (this.progress > 90) this.progress = 90;
      }
    });
  }

  private finishProgress() {
    if (this.timer) this.timer.unsubscribe();
    this.progress = 100;
    setTimeout(() => (this.progress = 0), 500);
  }

  ngOnDestroy() {
    this.timer?.unsubscribe();
  }
}
