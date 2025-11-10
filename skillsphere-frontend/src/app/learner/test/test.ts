import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Footer } from '../../common/footer/footer';
import { Navbar } from '../../common/navbar/navbar';
import { TestService } from '../../services/test.service';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';

@Component({
  selector: 'app-test',
  imports: [CommonModule, Navbar, Footer],
  templateUrl: './test.html',
  styleUrl: './test.css'
})
export class Test {
  tests: any;
  loading = true;
  error: string | null = null;

  constructor(private testService: TestService, private authService: AuthService, private toasterService: ToasterService) {}

  ngOnInit() {
    this.getTests();
  }

  getTests() {
    this.testService.getAvailableTests().subscribe({
      next: (response) => {
        this.tests = response;
      },
      error: () => {
        this.toasterService.error("Unable to load the tests.");
      }
    });
  }

  onParticipate(testId: number) {
    console.log(`Participate clicked for Test ID: ${testId}`);
  }
}
