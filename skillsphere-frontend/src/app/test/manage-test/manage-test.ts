import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TestService } from '../../services/test-service';

@Component({
  selector: 'app-manage-test',
  imports: [CommonModule],
  templateUrl: './manage-test.html',
  styleUrl: './manage-test.css'
})
export class ManageTest implements OnInit {
  tests : any[] = [];

  constructor(private readonly testService: TestService){}

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
}
