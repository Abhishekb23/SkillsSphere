import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { TestService } from '../../services/test-service';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-get-test',
  standalone: true,
  imports: [CommonModule, FormsModule, MatCardModule, MatCheckboxModule, MatRadioModule],
  templateUrl: './get-test.html',
  styleUrl: './get-test.css'
})
export class GetTest implements OnInit {
  testId!: number;
  testData: any;
  userId = 1; // ✅ Replace with actual logged-in user ID later
selectedAnswers: Record<number, number[]> = {}; // tracks all answers

  constructor(
    private readonly testService: TestService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.getTestById(this.testId);
  }

  getTestById(id: number) {
    this.testService.getTestById(id).subscribe({
      next: (res) => {
        this.testData = res;
        console.log('Test Data:', res);
      },
      error: (error) => {
        console.error('Error loading test:', error);
      }
    });
  }

  // ✅ For Single Choice (Radio)
  onSingleChoiceSelect(questionId: number, optionId: number) {
    this.selectedAnswers[questionId] = [optionId]; // Only one answer
  }

  // ✅ For Multiple Choice (Checkbox)
  onMultipleChoiceChange(questionId: number, optionId: number, event: any) {
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
  submitTest() {
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
