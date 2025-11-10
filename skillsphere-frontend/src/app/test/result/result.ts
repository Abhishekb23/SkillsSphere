import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TestService } from '../../services/test-service';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-result',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './result.html',
  styleUrls: ['./result.css'],
})
export class Result implements OnInit {
  results: any[] = [];
  isLoading = false;
  userId!: number;

  constructor(
    private readonly testService: TestService,
    private readonly authService: AuthService
  ) {}

  ngOnInit(): void {
    // ✅ Get logged-in user ID
    this.userId = this.authService.getUserId() ?? 0;
    this.loadResults();
  }

  /** ✅ Fetch results for logged-in user */
  private loadResults(): void {
    this.isLoading = true;

    this.testService.getUserResults(this.userId).subscribe({
      next: (res) => {
        this.results = res.sort(
          (a: any, b: any) => new Date(b.completedAt).getTime() - new Date(a.completedAt).getTime()
        );
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error loading results:', err);
        this.isLoading = false;
      },
    });
  }

  /** ✅ Format date/time */
  formatDate(date: string): string {
    return new Date(date).toLocaleString();
  }

  /** ✅ Get performance color based on score */
  getScoreClass(score: number): string {
    if (score >= 80) return 'bg-success';
    if (score >= 50) return 'bg-warning';
    return 'bg-danger';
  }
}
