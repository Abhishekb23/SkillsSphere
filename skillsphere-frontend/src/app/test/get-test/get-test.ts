import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { TestService } from '../../services/test-service';
import { AuthService } from '../../services/auth-service';
@Component({
  selector: 'app-get-test',
  standalone: true,
  imports: [CommonModule, FormsModule, MatCardModule, MatCheckboxModule, MatRadioModule],
  templateUrl: './get-test.html',
  styleUrls: ['./get-test.css']
})
export class GetTest implements OnInit {
  testId!: number;
  testData: any;
  isAdmin: boolean = false;
  isLearner: boolean = false;
  userId: number = 1; // ✅ Replace later with decoded userId from JWT
  isLoading: boolean = false;

  selectedAnswers: Record<number, number[]> = {}; // tracks all answers

  constructor(
    private readonly testService: TestService,
    private readonly authService: AuthService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.setUserRole();
    this.getTestById(this.testId);
  }

  /** ✅ Determine user role */
  private setUserRole(): void {
    this.isAdmin = this.authService.isAdmin();
    this.isLearner = this.authService.isLearner();
  }

  /** ✅ Load test data (different API for Admin & Learner) */
  private getTestById(id: number): void {
    this.isLoading = true;

    const apiCall = this.isAdmin
      ? this.testService.getTestById(id)
      : this.testService.getLearnerTestById(id);

    apiCall.subscribe({
      next: (res) => {
        this.testData = res;
        this.isLoading = false;
        console.log('Loaded test:', res);
      },
      error: (error) => {
        this.isLoading = false;
        console.error('Error loading test:', error);
        alert('Failed to load test.');
      }
    });
  }

  // ✅ For Single Choice (Radio)
  onSingleChoiceSelect(questionId: number, optionId: number): void {
    this.selectedAnswers[questionId] = [optionId];
  }

  // ✅ For Multiple Choice (Checkbox)
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

  // ✅ Submit Test
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

    console.log('Submitting payload:', payload);

    this.testService.submitTest(payload).subscribe({
      next: (res) => {
        alert('✅ Test submitted successfully!');
        console.log('Response:', res);
      },
      error: (err) => {
        console.error('Submission failed:', err);
        alert('❌ Failed to submit test.');
      }
    });
  }
}
