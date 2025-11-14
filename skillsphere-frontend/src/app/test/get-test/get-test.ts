import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TestService } from '../../services/test.service';
import { AuthService } from '../../services/auth.service';
import { ToasterService } from '../../services/toaster.service';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-get-test',
  standalone: true,
  imports: [CommonModule, FormsModule, MatCardModule, MatCheckboxModule, MatRadioModule,MatIconModule],
  templateUrl: './get-test.html',
  styleUrls: ['./get-test.css']
})
export class GetTest implements OnInit {
  testId!: number;
  testData: any;
  isAdmin = false;
  isLearner = false;
  userId = 1; // ✅ Replace with decoded userId later
  isLoading = false;
  thumbnailUrl: string | null = null; // ✅ new property

  selectedAnswers: Record<number, number[]> = {}; // tracks all answers

  constructor(
    public readonly testService: TestService,
    private readonly authService: AuthService,
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly toasterService:ToasterService
  ) {}

  ngOnInit(): void {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.setUserRole();
    this.getTestById(this.testId);
  }

  editTest(): void {
  this.router.navigate(['/create-test'], {
    queryParams: { id: this.testData.testId }
  });
}

  private setUserRole(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isLearner = this.authService.isLearner();
  }

  /** ✅ Load test and thumbnail */
  private getTestById(id: number): void {
    this.isLoading = true;

    const apiCall = this.isAdmin
      ? this.testService.getTestById(id)
      : this.testService.getLearnerTestById(id);

    apiCall.subscribe({
      next: (res) => {
        this.testData = res;
        this.isLoading = false;

        // ✅ Fetch thumbnail once test data is loaded
        this.testService.getThumbnail(this.testData.testId).subscribe({
          next: (base64) => (this.thumbnailUrl = base64),
          error: () => (this.thumbnailUrl = null)
        });
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error loading test:', error);
        alert('Failed to load test.');
      }
    });
  }

  onSingleChoiceSelect(questionId: number, optionId: number): void {
    this.selectedAnswers[questionId] = [optionId];
  }

  onMultipleChoiceChange(questionId: number, optionId: number, event: any): void {
    if (!this.selectedAnswers[questionId]) {
      this.selectedAnswers[questionId] = [];
    }

    if (event.checked) {
      this.selectedAnswers[questionId].push(optionId);
    } else {
      this.selectedAnswers[questionId] = this.selectedAnswers[questionId].filter(id => id !== optionId);
    }
  }

  submitTest(): void {
    if (!this.testData) return;

    const payload = {
      testId: this.testData.testId,
      userId: this.userId,
      answers: Object.entries(this.selectedAnswers).map(([qId, optionIds]) => ({
        questionId: Number(qId),
        selectedOptionIds: optionIds
      }))
    };

    this.testService.submitTest(payload).subscribe({
      next: () =>this.toasterService.information('Test submitted!'),
      error: () =>this.toasterService.error('Test Submission faild')
    });
  }


  deleteTest(){
    this.testService.deleteTest(this.testId).subscribe({
    next:(res)=>{
      this.toasterService.success(`${this.testData.title} deleted successfully`);
      this.router.navigate(['/manage-tests'])
    },
    error:(err)=>{
      this.toasterService.error(`${this.testData.title} not deleted`);
    }
    });
  }
}
