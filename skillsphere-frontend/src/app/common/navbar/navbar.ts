import { AfterViewInit, Component, Input } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';

@Component({
  selector: 'app-navbar',
  imports: [FormsModule, RouterModule, CommonModule, MatIconModule, MatMenuModule],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class Navbar implements AfterViewInit {
  isAdmin: boolean = false;
  isAuthenticated: boolean = false;
  userName: string | null = '';
  userInitials: string = '';
  menuOpen = false;
  isMobileMenu = false;

  constructor(private authService: AuthService, private readonly router: Router) {
    this.isAuthenticated = authService.isAuthenticated();
    this.isAdmin = authService.isAdmin();
  }

  ngOnInit() {
    var userName = this.authService.getUserName() ?? "";
    this.userInitials = this.getInitials(userName);
  }

  logout() {
    this.isAuthenticated = false;
    this.menuOpen = false;
    this.authService.logout();
  }

  ngAfterViewInit() {
    const navbar = document.querySelector('.navbar');
    const navToggle = document.getElementById('nav-toggle');
    const navLinks = document.querySelector('.nav-links');

    window.addEventListener('scroll', () => {
      if (window.scrollY > 50) navbar?.classList.add('scrolled');
      else navbar?.classList.remove('scrolled');
    });

    navToggle?.addEventListener('click', () => {
      navLinks?.classList.toggle('open');
    });
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  toggleMobileMenu() {
    this.isMobileMenu = !this.isMobileMenu;
  }

  getInitials(name: string): string {
    const parts = name.trim().split(' ');
    if (parts.length >= 2) {
      return (parts[0][0] + parts[1][0]).toUpperCase();
    }
    return name.substring(0, 2).toUpperCase();
  }
}
