import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SboModelComponent } from './sbo-model.component';

describe('SboModelComponent', () => {
  let component: SboModelComponent;
  let fixture: ComponentFixture<SboModelComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SboModelComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SboModelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
