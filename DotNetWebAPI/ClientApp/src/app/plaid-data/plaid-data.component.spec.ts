import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlaidDataComponent } from './plaid-data.component';

describe('PlaidDataComponent', () => {
  let component: PlaidDataComponent;
  let fixture: ComponentFixture<PlaidDataComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlaidDataComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlaidDataComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
