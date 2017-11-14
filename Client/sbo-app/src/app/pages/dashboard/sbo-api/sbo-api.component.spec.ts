import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SboApiComponent } from './sbo-api.component';

describe('SboApiComponent', () => {
  let component: SboApiComponent;
  let fixture: ComponentFixture<SboApiComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SboApiComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SboApiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
