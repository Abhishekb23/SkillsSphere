import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

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
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard {

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

  ngOnInit(): void {
    setInterval(() => {
      this.currentSlide = (this.currentSlide + 1) % this.imageSlides.length;
    }, 3500); // every 3.5 seconds
  }
}
