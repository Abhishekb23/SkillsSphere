import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth-service';
import { Router, RouterModule } from '@angular/router';
import { ToasterService } from '../../services/toaster-service';


@Component({
  selector: 'app-signup',
  imports: [FormsModule,RouterModule],
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

  constructor(private readonly authService: AuthService,
    private toasterService: ToasterService,
    private router: Router
  ){}

  onSignup() {
    this.authService.signup(this.user).subscribe({
      next: (res) => {
        this.toasterService.success('Account created successfully');
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.log(error);
        this.toasterService.success(error.error?.message);
      }
    });
  }
}
