import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { Router, RouterModule } from '@angular/router';
import { ToasterService } from '../../services/toaster-service';

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

  constructor(private readonly authService: AuthService,private router:Router, private toasterService: ToasterService){}

  onLogin() {
    this.authService.login(this.user).subscribe({
      next: (res) => {
        this.token = res.token;
        this.authService.setToken(this.token);
        this.toasterService.success('Logged in successfully');
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.toasterService.error(error.error?.message);
      }
    });
  }
}
