import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseService } from '../../services/course.service';
import { FormsModule } from '@angular/forms';

interface Course {
  courseId: number;
  title: string;
  description: string;
  thumbnailUrl?: string;
  createdBy: number;
  createdAt: string;
  isActive: boolean;
  modules?: any[];
}

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './manage-course.html',
  styleUrls: ['./manage-course.css'],
})
export class ManageCourses implements OnInit {
  courses: Course[] = [];
  isLoading = true;

  constructor(private course: CourseService) {}

  ngOnInit(): void {
    this.getCourses();
  }

  getCourses() {
    this.course.getCoursesForLearner().subscribe({
      next: (res) => {
        this.courses = res;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Error fetching courses:', err);
        this.isLoading = false;
      },
    });
  }

  viewCourse(courseId: number) {
    console.log('View course:', courseId);    
  }
}
