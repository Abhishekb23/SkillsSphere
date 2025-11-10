import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';
import { Footer } from '../footer/footer';
import { Navbar } from '../navbar/navbar';

@Component({
  selector: 'app-profile',
  imports: [Footer, Navbar, CommonModule, FormsModule, RouterLink],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
  isEditing = false;

  user = {
    name: '',
    email: '',
    phone: '',
    dob: '',
    about: '',
    skills: '',
    profile_url: ''
  };

  constructor(
    private readonly authService: AuthService,
    private readonly toaster: ToasterService
  ) {
    this.loadUser();
  }

  loadUser() {   
    this.user.name = this.authService.getUserName() ?? "";
    this.user.email = this.authService.getUserEmail() ?? "NA";
    this.user.phone = "NA";
    this.user.dob = "NA";
    this.user.about = "NA";
    this.user.skills = "NA";
    this.user.profile_url = "https://i.pravatar.cc/150?img=12";        
  }

  toggleEdit() {
    this.isEditing = !this.isEditing;
  }

  saveProfile() {
    // this.authService.updateProfile(this.user).subscribe({
    //   next: (res) => {
    //     this.toaster.success('Profile updated successfully!');
    //     this.isEditing = false;
    //   },
    //   error: (err) => {
    //     this.toaster.error(err.error?.message || 'Failed to update profile');
    //   }
    // });
  }
}
