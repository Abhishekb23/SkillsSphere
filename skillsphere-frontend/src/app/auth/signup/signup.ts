import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';


@Component({
  selector: 'app-signup',
  imports: [FormsModule],
  templateUrl: './signup.html',
  styleUrl: './signup.css'
})
export class Signup {
   user = {
    username: '',
    email: '',
    password: '',
    role: 1
  };

  constructor(private readonly authService: AuthService){}

  onSignup() {
    this.authService.signup(this.user).subscribe({
      next: (res) => {
        console.log(res);
      },
      error: (error) => {
        console.log(error);
      }
    });
  }
}
