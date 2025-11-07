import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCard, MatCardModule } from '@angular/material/card';
import { RouterModule } from '@angular/router';
import { AuthService } from '../services/auth-service';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { TestService } from '../services/test-service';
import { Navbar } from "../common/navbar/navbar";

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
  imports: [CommonModule, RouterModule, MatCard, MatIconModule, MatButtonModule,
    MatMenuModule, MatDividerModule, MatCardModule, MatProgressBarModule, Navbar],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

  isAuthenticated: boolean = false;
  isAdmin: boolean = false;
  userName: string | null = null;
  testsCount: number = 0;
  fullText = 'Become the next generation developer';
  displayedText = '';
  typingSpeed = 100;

  featuredCourses: CourseCard[] = [
    {
      title: 'Angular from Scratch',
      instructor: 'John Doe',
      img: 'assets/course1.jpg',
      price: '$19.99'
    },
    {
      title: 'Advanced CSS Animations',
      instructor: 'Jane Smith',
      img: 'assets/course2.jpg',
      price: '$24.99'
    },
    {
      title: 'Python for Data Science',
      instructor: 'Alice Brown',
      img: 'assets/course3.jpg',
      price: '$29.99'
    }
  ];

  imageSlides: string[] = [
    'https://www.w3schools.com/howto/img_nature_wide.jpg',
    'https://www.w3schools.com/howto/img_snow_wide.jpg',
    'https://www.w3schools.com/howto/img_mountains_wide.jpg'
  ];

  currentSlide = 0;

  constructor(private readonly authService: AuthService, private readonly testService: TestService) { }

  ngOnInit(): void {
    this.isAuthenticated = this.authService.isAuthenticated();
    this.isAdmin = this.authService.isAdmin();
    this.userName = this.authService.getUserName();

    if (this.isAuthenticated) {
      this.getTestCounts();
    } else {
      this.startTyping();
    }
    
    setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % this.imageSlides.length;
    }, 3500); // every 3.5 seconds
  }

  startTyping() {
    let index = 0;
    const interval = setInterval(() => {
      if (index < this.fullText.length) {
        this.displayedText += this.fullText[index];
        index++;
      } else {
        clearInterval(interval);
      }
    }, this.typingSpeed);
  }

  getTestCounts() {
    this.testService.getTestsCount().subscribe({
      next: (res) => {
        this.testsCount = res;
        console.log(this.testsCount);
      }, error: (error) => {
        console.error(error);
      }
    });
  }
}
