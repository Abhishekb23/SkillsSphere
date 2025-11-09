import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';

@Component({
  selector: 'app-admin',
  imports: [CommonModule],
  templateUrl: './admin.html',
  styleUrl: './admin.css'
})
export class Admin {
  users = [
    { name: 'Vivek Kushwaha', email: 'vivek@gmail.com', role: 'Learner', status: 'Active' },
    { name: 'Aditi Mehta', email: 'aditi@gmail.com', role: 'Instructor', status: 'Active' },
    { name: 'Rahul Verma', email: 'rahul@gmail.com', role: 'Learner', status: 'Inactive' },
  ];

  courses = [
    { title: 'Full Stack Web Dev', learners: 240, status: 'Ongoing' },
    { title: 'SQL & Databases', learners: 180, status: 'Completed' },
    { title: 'Angular Mastery', learners: 150, status: 'Ongoing' },
  ];
}
