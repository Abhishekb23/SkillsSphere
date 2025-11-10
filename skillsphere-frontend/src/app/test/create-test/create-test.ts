import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TestService } from '../../services/test-service';
import { ToasterService } from '../../services/toaster-service';

@Component({
  selector: 'app-create-test',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-test.html',
  styleUrls: ['./create-test.css']
})
export class CreateTest implements OnInit {
  quizForm: FormGroup;
  selectedThumbnail: File | null = null;
  thumbnailPreview: string | null = null;
  isSubmitting = false;
  isEditMode = false;
  testIdToEdit: number | null = null;

  constructor(
    private fb: FormBuilder,
    private readonly testService: TestService,
    private readonly toasterService: ToasterService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {
    this.quizForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      questions: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      if (params['id']) {
        this.isEditMode = true;
        this.testIdToEdit = +params['id'];
        this.loadTestData(this.testIdToEdit);
      } else {
        this.addQuestion(); // default empty question
      }
    });
  }

  // ✅ Load existing test for editing
  private loadTestData(testId: number) {
    this.testService.getTestById(testId).subscribe({
      next: (test) => {
        this.quizForm.patchValue({
          title: test.title,
          description: test.description
        });

        const questionsArray = this.quizForm.get('questions') as FormArray;
        questionsArray.clear();

        test.questions.forEach((q: any) => {
          const questionGroup = this.fb.group({
            questionId: [q.questionId],
            questionText: [q.questionText, Validators.required],
            questionType: [q.questionType, Validators.required],
            options: this.fb.array([])
          });

          q.options.forEach((opt: any) => {
            (questionGroup.get('options') as FormArray).push(
              this.fb.group({
                optionId: [opt.optionId],
                optionText: [opt.optionText, Validators.required],
                isCorrect: [opt.isCorrect]
              })
            );
          });

          questionsArray.push(questionGroup);
        });

        // ✅ Load existing thumbnail
        this.testService.getThumbnail(testId).subscribe({
          next: (thumb) => (this.thumbnailPreview = thumb),
          error: () => (this.thumbnailPreview = null)
        });
      },
      error: () => {
        this.toasterService.error('Failed to load test for editing.');
      }
    });
  }

  // ✅ Form helpers
  get questions(): FormArray {
    return this.quizForm.get('questions') as FormArray;
  }

  initOption(): FormGroup {
    return this.fb.group({
      optionText: ['', Validators.required],
      isCorrect: [false]
    });
  }

  addQuestion() {
    const questionGroup = this.fb.group({
      questionText: ['', Validators.required],
      questionType: ['SingleChoice', Validators.required],
      options: this.fb.array([this.initOption()])
    });
    this.questions.push(questionGroup);
  }

  removeQuestion(index: number) {
    this.questions.removeAt(index);
  }

  getOptions(index: number): FormArray {
    return this.questions.at(index).get('options') as FormArray;
  }

  addOption(qIndex: number) {
    this.getOptions(qIndex).push(this.initOption());
  }

  removeOption(qIndex: number, oIndex: number) {
    this.getOptions(qIndex).removeAt(oIndex);
  }

  // ✅ Thumbnail selection
  onThumbnailSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.selectedThumbnail = file;
      const reader = new FileReader();
      reader.onload = () => (this.thumbnailPreview = reader.result as string);
      reader.readAsDataURL(file);
    }
  }

  // ✅ Submit (Create or Update)
  submitQuiz() {
    if (this.quizForm.invalid) {
      this.toasterService.error('Please fill all required fields.');
      return;
    }

    this.isSubmitting = true;

    if (this.isEditMode && this.testIdToEdit) {
      this.updateTest(this.testIdToEdit);
    } else {
      this.createTest();
    }
  }

  // 🟩 Create new test
  private createTest() {
    this.testService.create(this.quizForm.value).subscribe({
      next: (res) => {
        const testId = res.testId || res.TestId;
        this.toasterService.success('Test created successfully!');

        if (this.selectedThumbnail && testId) {
          this.uploadThumbnail(testId);
        } else {
          this.resetForm();
        }
      },
      error: () => {
        this.toasterService.error('Failed to create test.');
        this.isSubmitting = false;
      }
    });
  }

  // 🟨 Update existing test
  private updateTest(testId: number) {
    const payload = { ...this.quizForm.value, testId };

    this.testService.updateTest(payload).subscribe({
      next: () => {
        this.toasterService.success('Test updated successfully!');
        if (this.selectedThumbnail) {
          this.uploadThumbnail(testId);
        } else {
          this.router.navigate(['/get-test', testId]);
          this.isSubmitting = false;
        }
      },
      error: () => {
        this.toasterService.error('Failed to update test.');
        this.isSubmitting = false;
      }
    });
  }

  // ✅ Upload thumbnail after create/update
  private uploadThumbnail(testId: number) {
    this.testService.uploadThumbnail(testId, this.selectedThumbnail!).subscribe({
      next: () => {
        this.toasterService.success('Thumbnail uploaded successfully!');
        this.router.navigate(['/get-test', testId]);
        this.resetForm();
      },
      error: () => {
        this.toasterService.error('Test saved, but thumbnail upload failed.');
        this.router.navigate(['/get-test', testId]);
      }
    });
  }

  private resetForm() {
    this.quizForm.reset();
    this.thumbnailPreview = null;
    this.selectedThumbnail = null;
    this.isSubmitting = false;
  }
}
