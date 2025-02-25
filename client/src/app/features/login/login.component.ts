import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  user: User;
  authService = inject(AuthService);
  router = inject(Router);

  ngOnInit(): void {
    this.authService.login('jv2@test.com', 'Pa$$w0rd').subscribe({
      next: () => {
        console.log(this.authService.user());
        // this.router.navigate(['/shop']);
      },
      error: error => console.error(error)
    })
  }
}
