import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetTest } from './get-test';

describe('GetTest', () => {
  let component: GetTest;
  let fixture: ComponentFixture<GetTest>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetTest]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetTest);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
