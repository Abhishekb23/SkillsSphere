import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatRadioModule } from '@angular/material/radio';
import { TestService } from '../../services/test-service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-get-test',
  imports: [CommonModule,MatCardModule,MatCheckboxModule,MatRadioModule],
  templateUrl: './get-test.html',
  styleUrl: './get-test.css'
})
export class GetTest implements OnInit{
  testId: any;
  testData:any;

  constructor(private readonly testService: TestService,private route: ActivatedRoute){    
  }

  ngOnInit(): void {
    this.testId = Number(this.route.snapshot.paramMap.get('testId'));
    this.getTestById(this.testId);
  }

  getTestById(id: number){
    this.testService.getTestById(id).subscribe({
      next: (res) => { 
        this.testData = res;
        console.log(res);
      },error: (error) => {
        console.log(error);
      }
    });
  }
}
