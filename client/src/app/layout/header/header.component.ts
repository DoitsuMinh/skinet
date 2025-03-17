import { Component, inject } from '@angular/core';
import { MatIcon } from '@angular/material/icon'
import { MatButton } from '@angular/material/button';
import { MatProgressBar } from '@angular/material/progress-bar';
import { MatBadge } from '@angular/material/badge'
import { AuthService } from 'src/app/core/services/auth.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { SnackbarService } from 'src/app/core/services/snackbar.service';
import { BusyService } from 'src/app/core/services/busy.service';
import { CartService } from 'src/app/core/services/cart.service';
import { MatMenu, MatMenuItem, MatMenuTrigger } from '@angular/material/menu';
import { MatDivider } from '@angular/material/divider';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    MatIcon,
    MatButton,
    MatBadge,
    RouterLink,
    RouterLinkActive,
    MatProgressBar,
    MatMenu,
    MatDivider,
    MatMenuItem,
    MatMenuTrigger
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  authService = inject(AuthService);
  private router = inject(Router);
  snackBar = inject(SnackbarService);
  busyService = inject(BusyService);
  cartService = inject(CartService);

  onLogOut(): void {
    this.authService.logout().subscribe({
      next: () => {
        this.authService.currentUser.set(null);
        this.snackBar.success('Logout successful!');
        this.router.navigate(['/'])
      }
    });

    // if (!this.authService.isLoggedIn()) {
    //  
    // }
    // this.router.navigate(['account/login']);
  }
}
