import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TestService } from '../../services/test-service';

@Component({
  selector: 'app-create-test',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './create-test.html',
  styleUrl: './create-test.css'
})
export class CreateTest {
  quizForm: FormGroup;

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
      questionType: ['single', Validators.required],
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

  submitQuiz() {
    if (this.quizForm.valid) {
      this.testService.create(this.quizForm.value).subscribe({
        next: (res)=>{
          alert(res);
        },
        error: (error) =>{
          alert(error);
        }
      });
    } else {
      alert('Please fill all required fields.');
    }
  }
}
