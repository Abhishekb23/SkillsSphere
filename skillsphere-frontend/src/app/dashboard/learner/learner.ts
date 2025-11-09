import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-learner',
  imports: [CommonModule],
  templateUrl: './learner.html',
  styleUrl: './learner.css'
})
export class Learner implements OnInit{
  isAuthenticated: boolean = false;
  isLearner: boolean = false;
  fullText: string = '';
  displayedText: string = '';
  userName: string = '';

  constructor(private authService: AuthService){
    this.isAuthenticated = authService.isAuthenticated();
    this.isLearner = authService.isLearner();
    this.userName = authService.getUserName()??"";
  }

  ngOnInit(): void {
    this.fullText = `Welcome back, ${this.userName}👋`;
    if(this.isAuthenticated){
      this.typeText();
    }
  }

   learnerName = 'Vivek Kushwaha';
  enrolledCourses = [
    { title: 'Full Stack Web Development', progress: 75, status: 'Ongoing' },
    { title: 'SQL & Databases', progress: 100, status: 'Completed' },
    { title: 'Machine Learning Basics', progress: 45, status: 'Ongoing' }
  ];

  upcomingTests = [
    { title: 'JavaScript Fundamentals', date: '15 Nov 2025', status: 'Scheduled' },
    { title: 'Data Structures Final', date: '22 Nov 2025', status: 'Pending' }
  ];

  certificates = [
    { title: 'SQL Mastery', issued: '01 Oct 2025' },
    { title: 'Web Development Basics', issued: '12 Sep 2025' }
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
