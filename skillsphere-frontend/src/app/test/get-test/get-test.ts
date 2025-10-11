import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { TestService } from '../../services/test-service';

@Component({
  selector: 'app-get-test',
  imports: [CommonModule,MatCardModule,MatCheckboxModule,MatRadioModule],
  templateUrl: './get-test.html',
  styleUrl: './get-test.css'
})
export class GetTest {
  testData:any;

  constructor(private readonly testService: TestService){
    this.getTestById();
  }

  getTestById(){
    this.testService.getTestById(1).subscribe({
      next: (res) => { 
        this.testData = res;
      },error: (error) => {
        alert(error);
      }
    });
  }
}
