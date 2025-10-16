import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth-service';
import { Toaster } from "./common/toaster/toaster";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule, RouterModule, CommonModule, Toaster],
  templateUrl: './app.html',
  styleUrls: ['./app.css'], 
})
export class App {
  isAdmin: boolean = false;
  isAuthenticated: boolean = false;

  constructor(public authService: AuthService) {
    this.isAuthenticated = authService.isAuthenticated();
    this.isAdmin = authService.isAdmin();    
  } // make it public for template access
  
}
