import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner'
import { MatButton } from '@angular/material/button';

@Component({
  standalone: true,
  selector: 'app-checkout-success',
  imports: [
    RouterLink,
    MatProgressSpinnerModule,
    MatButton
  ],
  templateUrl: './checkout-success.component.html',
  styleUrl: './checkout-success.component.scss'
})
export class CheckoutSuccessComponent {

}
