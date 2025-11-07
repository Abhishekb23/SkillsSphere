import { Component, Input } from '@angular/core';
import { AuthService } from '../../services/auth-service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { Toaster } from '../toaster/toaster';

@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterModule, CommonModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar {
  isAdmin: boolean = false;
  isAuthenticated: boolean = false;
  @Input() userName: string | null = '';

  constructor(public authService: AuthService) {
    this.isAuthenticated = authService.isAuthenticated();
    this.isAdmin = authService.isAdmin();
  }
}
