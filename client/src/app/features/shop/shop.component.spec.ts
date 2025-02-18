import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopComponent } from './shop.component';
import { provideHttpClient } from '@angular/common/http';

describe('ShopComponent', () => {
  let component: ShopComponent;
  let fixture: ComponentFixture<ShopComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShopComponent],
      providers: [provideHttpClient()]
    })
      .compileComponents();

    fixture = TestBed.createComponent(ShopComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call loadProducts on initialization', () => {
    spyOn(component, 'loadProducts'); // Spy on loadProducts
    component.ngOnInit(); // Call ngOnInit, which now calls loadProducts
    expect(component.loadProducts).toHaveBeenCalled(); // Check if loadProducts was called
  });
});
