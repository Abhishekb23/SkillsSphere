import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageTest } from './manage-test';

describe('ManageTest', () => {
  let component: ManageTest;
  let fixture: ComponentFixture<ManageTest>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ManageTest]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ManageTest);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
