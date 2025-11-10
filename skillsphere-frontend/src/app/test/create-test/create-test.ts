import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TestService } from '../../services/test-service';

@Component({
  selector: 'app-create-test',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-test.html',
  styleUrls: ['./create-test.css']
})
export class CreateTest {
  quizForm: FormGroup;
  selectedThumbnail: File | null = null;
  thumbnailPreview: string | null = null;
  isSubmitting = false;

  constructor(private fb: FormBuilder, private readonly testService: TestService) {
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      questions: this.fb.array([this.initQuestion()])
    });
  }

  get questions(): FormArray {
    return this.quizForm.get('questions') as FormArray;
  }

  initQuestion(): FormGroup {
    return this.fb.group({
      questionText: ['', Validators.required],
      questionType: ['SingleChoice', Validators.required],
      options: this.fb.array([this.initOption()])
    });
  }

  initOption(): FormGroup {
    return this.fb.group({
      optionText: ['', Validators.required],
      isCorrect: [false]
    });
  }

  addQuestion() {
    this.questions.push(this.initQuestion());
  }

  removeQuestion(index: number) {
    this.questions.removeAt(index);
  }

  getOptions(questionIndex: number): FormArray {
    return this.questions.at(questionIndex).get('options') as FormArray;
  }

  addOption(questionIndex: number) {
    this.getOptions(questionIndex).push(this.initOption());
  }

  removeOption(questionIndex: number, optionIndex: number) {
    this.getOptions(questionIndex).removeAt(optionIndex);
  }

  // ✅ Thumbnail selection + preview
  onThumbnailSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedThumbnail = file;

      // Show preview
      const reader = new FileReader();
      reader.onload = () => {
        this.thumbnailPreview = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

  // ✅ Submit quiz (create test + upload thumbnail)
  submitQuiz() {
    if (this.quizForm.invalid) {
      alert('Please fill all required fields.');
      return;
    }

    this.isSubmitting = true;

    // Step 1️⃣ — create test
    this.testService.create(this.quizForm.value).subscribe({
      next: (res) => {
        const testId = res.testId || res.TestId;
        alert('Test created successfully!');

        // Step 2️⃣ — upload thumbnail if selected
        if (this.selectedThumbnail && testId) {
          this.testService.uploadThumbnail(testId, this.selectedThumbnail).subscribe({
            next: () => {
              alert('Thumbnail uploaded successfully!');
              this.isSubmitting = false;
              this.quizForm.reset();
              this.thumbnailPreview = null;
              this.selectedThumbnail = null;
            },
            error: (err) => {
              console.error(err);
              alert('Test created, but failed to upload thumbnail.');
              this.isSubmitting = false;
            }
          });
        } else {
          this.isSubmitting = false;
        }
      },
      error: (error) => {
        console.error(error);
        alert('Failed to create test.');
        this.isSubmitting = false;
      }
    });
  }
}
