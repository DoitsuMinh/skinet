import { TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { HttpClient, provideHttpClient } from '@angular/common/http';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [provideHttpClient()] //  Ensure HttpClient is provided
    });
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it(`should have as title 'skinet'`, () => {
    const app = fixture.componentInstance;
    expect(app.title).toEqual('skinet');
  });

});
