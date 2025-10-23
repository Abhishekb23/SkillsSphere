import { Component, OnInit } from '@angular/core';
import { TestService } from '../../services/test-service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manage-test',
  imports: [CommonModule],
  templateUrl: './manage-test.html',
  styleUrl: './manage-test.css'
})
export class ManageTest implements OnInit {
  tests : any[] = [];

  constructor(private readonly testService: TestService, private readonly router: Router){}

  ngOnInit(): void {
    this.getAllTests();
  }


  getAllTests(){
    this.testService.getList().subscribe({
        next: (res) => {
          this.tests = res;
          console.log(res);
        },
        error: (error) => {
          alert(error);
        }
      });
  }

  attendTest(testId: number){
    this.router.navigate(['get-test', testId]); 
  }
}
