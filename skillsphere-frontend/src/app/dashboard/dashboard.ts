import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCard, MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TestService } from '../services/test.service';
import { Navbar } from "../common/navbar/navbar";
import { Footer } from "../common/footer/footer";
import { Admin } from "./admin/admin";
import { Learner } from "./learner/learner";

interface Slide {
  title: string;
  subtitle: string;
  img: string;
  ctaText: string;
  ctaRoute: string;
}

interface CourseCard {
  title: string;
  instructor: string;
  img: string;
  price: string;
}

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterModule, MatIconModule, MatButtonModule,
    MatMenuModule, MatDividerModule, MatCardModule, MatProgressBarModule, Navbar, Footer, Admin, Learner],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {
  isAuthenticated: boolean = false;
  isAdmin: boolean = false;
  userName: string | null = null;
  testsCount: number = 0;

  constructor(private readonly authService: AuthService, private readonly testService: TestService) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.isAdmin = this.authService.isAdmin();
    this.userName = this.authService.getUserName();

    if (this.isAuthenticated) {
      this.getTestCounts();
    } else { }
  }

  getTestCounts() {
    this.testService.getTestsCount().subscribe({
      next: (res) => {
        this.testsCount = res;
      }, error: (error) => {
        console.error(error);
      }
    });
  }
}
