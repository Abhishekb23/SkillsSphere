import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { TestService } from '../../services/test.service';

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
    this.userId = this.authService.getUserId() ?? 0;
    this.loadResults();
  }

  private loadResults(): void {
    this.isLoading = true;

    this.testService.getUserResults(this.userId).subscribe({
      next: (res: any) => {
        this.results = res.sort(
          (a: any, b: any) => new Date(b.completedAt).getTime() - new Date(a.completedAt).getTime()
        );
        this.isLoading = false;
      },
      error: (err: any) => {
        console.error('Error loading results:', err);
        this.isLoading = false;
      },
    });
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleString();
  }

  getScoreClass(score: number): string {
    if (score >= 80) return 'bg-success';
    if (score >= 50) return 'bg-warning';
    return 'bg-danger';
  }
}
