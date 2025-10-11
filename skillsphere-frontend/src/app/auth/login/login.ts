import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';

@Component({
  selector: 'app-login',
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {
  user = {
    email: '',
    password: ''
  };

  private token: string = '';

  constructor(private readonly authService: AuthService){}

  onLogin() {
    this.authService.login(this.user).subscribe({
      next: (res) => {
        this.token = res;
        console.log(res);
        alert("success");
      },
      error: (error) => {
        alert("error");
      }
    });
  }
}
