import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-learner',
  imports: [CommonModule, RouterLink],
  templateUrl: './learner.html',
  styleUrl: './learner.css'
})
export class Learner implements OnInit {
  isAuthenticated: boolean = false;
  isLearner: boolean = false;
  fullText: string = '';
  displayedText: string = '';
  userName: string = '';

  constructor(private authService: AuthService) {
    this.isAuthenticated = authService.isAuthenticated();
    this.isLearner = authService.isLearner();
    this.userName = authService.getUserName() ?? "";
  }

  ngOnInit(): void {
    this.fullText = `Welcome back, ${this.userName}👋`;
    if (this.isAuthenticated) {
      this.typeText();
    }
  }

  enrolledCourses = [
    { title: 'SQL & Databases', progress: 100, status: 'Completed' },
  ];

  upcomingTests = [
    { title: 'Data Structures Final', date: '22 June 2026', status: 'Pending' }
  ];

  certificates = [
    { title: 'SQL & Databases', issued: '01 Oct 2025' },
  ];

  typeText() {
    let i = 0;
    const typingSpeed = 150;
    const interval = setInterval(() => {
      if (i < this.fullText.length) {
        this.displayedText += this.fullText.charAt(i);
        i++;
      } else {
        clearInterval(interval);
      }
    }, typingSpeed);
  }
}
