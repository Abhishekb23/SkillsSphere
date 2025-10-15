import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [FormsModule,RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user = {
    email: '',
    password: ''
  };

  private token: string = '';

  constructor(private readonly authService: AuthService,private router:Router){}

  onLogin() {
    this.authService.login(this.user).subscribe({
      next: (res) => {
        this.token = res.token;
        this.authService.setToken(this.token);
        this.router.navigate(['/']);
      },
      error: (error) => {
        alert("error");
      }
    });
  }
}
