import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile {
  isEditing = false;
  userId = 0;

  user = {
    name: '',
    email: '',
    phone: '',
    dob: '',
    about: '',
    skills: '',
    profile_url: '',
  };

  previewImage: string | null = null;
  selectedImage: File | null = null;

  constructor(
    private readonly userService: UserService,
    private readonly authService: AuthService,
    private readonly toaster: ToasterService
  ) {}

  ngOnInit() {
    this.userId = this.authService.getUserId() ?? 0;
    this.loadUserProfile();
  }

  /** LOAD PROFILE FROM API */
  loadUserProfile() {
    this.userService.getProfile(this.userId).subscribe({
      next: (res: any) => {
        this.user.name = res.fullName ?? '';
        this.user.email = res.email ?? '';
        this.user.phone = res.phone ?? '';
        this.user.dob = res.dateOfBirth ?? '';
        this.user.about = res.about ?? '';
        this.user.skills = res.skills ?? '';
        this.user.profile_url =
          res.profileImageBase64 ?? 'https://i.pravatar.cc/150';
      },
      error: () => {
        this.toaster.error('Failed to load profile');
      },
    });
  }

  /** SELECT IMAGE */
  onImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedImage = file;

      const reader = new FileReader();
      reader.onload = () => {
        this.previewImage = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  /** TOGGLE EDIT MODE */
  toggleEdit() {
    this.isEditing = !this.isEditing;

    // When cancelling edit → remove preview
    if (!this.isEditing) {
      this.previewImage = null;
      this.selectedImage = null;
    }
  }

  /** SAVE PROFILE TO API */
  saveProfile() {
    const formData = new FormData();
    formData.append('UserId', this.userId.toString());
    formData.append('FullName', this.user.name);
    formData.append('Phone', this.user.phone);
    formData.append('DateOfBirth', this.user.dob);
    formData.append('About', this.user.about);
    formData.append('Skills', this.user.skills);

    if (this.selectedImage) {
      formData.append('profileImage', this.selectedImage);
    }

    this.userService.saveProfile(formData).subscribe({
      next: () => {
        this.toaster.success('Profile updated successfully!');

        if (this.previewImage) {
          this.user.profile_url = this.previewImage;
        }

        this.previewImage = null;
        this.isEditing = false;
      },
      error: () => {
        this.toaster.error('Failed to update profile');
      },
    });
  }
}
