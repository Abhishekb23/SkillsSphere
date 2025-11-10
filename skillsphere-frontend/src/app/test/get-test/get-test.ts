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
  isAdmin = false;
  isLearner = false;
  userId = 1; // ✅ Replace with decoded userId later
  isLoading = false;
  thumbnailUrl: string | null = null; // ✅ new property

  selectedAnswers: Record<number, number[]> = {}; // tracks all answers

  constructor(
    public readonly testService: TestService,
    private readonly authService: AuthService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.setUserRole();
    this.getTestById(this.testId);
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
      next: () => alert('✅ Test submitted successfully!'),
      error: () => alert('❌ Failed to submit test.')
    });
  }
}
