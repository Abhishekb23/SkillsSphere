import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router, RouterModule } from '@angular/router';
import { ToasterService } from '../../services/toaster.service';
import { Navbar } from "../../common/navbar/navbar";
import { Footer } from "../../common/footer/footer";

@Component({
  selector: 'app-login',
  imports: [FormsModule, RouterModule, Navbar, Footer],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user = {
    email: '',
    password: ''
  };

  private token: string = '';

  constructor(
    private readonly authService: AuthService,
    private router: Router,
    private toasterService: ToasterService) {
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/']);
    }
  }

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
