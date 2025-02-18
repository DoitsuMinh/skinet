import { TestBed } from '@angular/core/testing';
import { ShopService } from '../shop.service';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';

describe('ShopService', () => {
  let service: ShopService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        ShopService,
        provideHttpClient(),
        provideHttpClientTesting()] // ✅ Add provideHttpClient() here
    });
    service = TestBed.inject(ShopService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
