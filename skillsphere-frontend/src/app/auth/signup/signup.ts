import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';
import { CommonModule } from '@angular/common';
import { MatIconModule } from "@angular/material/icon";
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [FormsModule, RouterModule, CommonModule, MatIconModule, MatButtonModule],
  templateUrl: './signup.html',
  styleUrls: ['./signup.css']
})
export class Signup {

  user = {
    username: '',
    email: '',
    password: '',
    confirmPassword: '',
  };

  otp: string = '';
  showOtpField: boolean = false;
  countdown = 300;
  timerInterval: any;
  disableVerifyButton: boolean = false;
  passwordMismatch: boolean = false;

  constructor(
    private readonly authService: AuthService,
    private readonly toasterService: ToasterService,
    private readonly router: Router
  ) { 
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/']);
    }
  }

  onSignup() {
    if(this.user.password !== this.user.confirmPassword){
      this.passwordMismatch = true;
      return;
    }
    var userData = {
      username: this.user.username,
      email: this.user.email,
      password: this.user.password,
      role: 1
    };
    this.authService.signup(userData).subscribe({
      next: (res) => {
        this.toasterService.success('OTP sent to your email');
        this.showOtpField = true;
        this.disableVerifyButton = false;
        this.startCountdown();
      },
      error: (error) => {
        console.log(error);
        this.toasterService.error(error.error?.message || 'Signup failed');
      }
    });
  }

  startCountdown() {
    this.countdown = 20; // reset timer
    clearInterval(this.timerInterval);
    this.timerInterval = setInterval(() => {
      if (this.countdown > 0) {
        this.countdown--;
      } else {
        clearInterval(this.timerInterval);
        this.disableVerifyButton = true;
        this.toasterService.error('OTP expired. Please sign up again.');
      }
    }, 1000);
  }

  onVerifyOtp() {
    const body = { email: this.user.email, otpCode: this.otp };
    this.authService.verifyOtp(body).subscribe({
      next: (res) => {
        console.log(res);
        this.toasterService.success('Account verified successfully!');
        clearInterval(this.timerInterval);
        this.router.navigate(['/login']);
      },
      error: (error) => {
        console.log(error);
        this.toasterService.error(error.error?.message || 'Invalid OTP');
      }
    });
  }

  get formattedTime(): string {
    const minutes = Math.floor(this.countdown / 60);
    const seconds = this.countdown % 60;
    return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
  }
}
