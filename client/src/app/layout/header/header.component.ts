import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon'
import { MatButton } from '@angular/material/button'
import { MatBadge } from '@angular/material/badge'
import { AuthService } from 'src/app/core/services/auth.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatIcon,
    MatButton,
    MatBadge,
    RouterLink,
    RouterLinkActive
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  authService = inject(AuthService);
  router = inject(Router);
  snackBar = inject(SnackbarService);
  readonly TIME_OUT = 500;

  onLogOut(): void {
    this.authService.logout();
    setTimeout(() => {
      if (!this.authService.isLoggedIn()) {
        this.snackBar.success('Logout successful!');

        this.router.navigate(['/login']);
      }
    }, this.TIME_OUT);
  }
}
