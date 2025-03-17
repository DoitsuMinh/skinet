import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {
  private snackbar = inject(MatSnackBar)

  error(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['snack-error']
    })
  }

  warn(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: 2000,
      panelClass: ['snack-warning']
    })
  }

  success(message: string) {
    this.snackbar.open(message, 'Close', {
      duration: 5000,
      panelClass: ['snack-success']
    })
  }
}
