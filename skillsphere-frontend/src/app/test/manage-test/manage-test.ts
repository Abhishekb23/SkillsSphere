import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { TestService } from '../../services/test.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-manage-test',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './manage-test.html',
  styleUrls: ['./manage-test.css']
})
export class ManageTest implements OnInit {
  tests: any[] = [];
  isAdmin: boolean = false;
  isLearner: boolean = false;
  isLoading: boolean = false;

  constructor(
    private readonly testService: TestService,
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.setUserRole();
    this.loadTests();
  }

  /** ✅ Determine the user's role using AuthService */
  private setUserRole(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isLearner = this.authService.isLearner();
  }

  /** ✅ Load data dynamically based on role */
  private loadTests(): void {
    this.isLoading = true;

    const apiCall = this.isAdmin
      ? this.testService.getAdminList()
      : this.testService.getAvailableTests();

    apiCall.subscribe({
      next: (res) => {
        this.tests = res;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tests:', error);
        alert('Failed to load tests');
        this.isLoading = false;
      },
    });
  }

  /** ✅ Handle test attendance */
  attendTest(testId: number): void {
    this.router.navigate(['get-test', testId]);
  }
}
