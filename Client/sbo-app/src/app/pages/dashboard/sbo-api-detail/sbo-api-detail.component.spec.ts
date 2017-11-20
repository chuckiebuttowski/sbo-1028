import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SboApiDetailComponent } from './sbo-api-detail.component';

describe('SboApiDetailComponent', () => {
  let component: SboApiDetailComponent;
  let fixture: ComponentFixture<SboApiDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SboApiDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SboApiDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
