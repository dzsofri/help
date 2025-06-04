import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MasikOldalComponent } from './masik-oldal.component';

describe('MasikOldalComponent', () => {
  let component: MasikOldalComponent;
  let fixture: ComponentFixture<MasikOldalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasikOldalComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasikOldalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
