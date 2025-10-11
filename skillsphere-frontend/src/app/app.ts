import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth-service';

@Component({
  selector: 'app-root',
  standalone: true, // ✅ include this if using standalone components
  imports: [RouterOutlet, FormsModule, RouterModule, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css'], // ✅ fixed plural
})
export class App {
  protected readonly title = signal('skillsphere-frontend');

  constructor(public authService: AuthService) {} // ✅ make it public for template access
  
}
