import { CommonModule, CurrencyPipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { CartService } from 'src/app/core/services/cart.service';

@Component({
  selector: 'app-order-summary',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatFormField,
    FormsModule,
    MatIcon,
    MatLabel,
    MatInput,
    CommonModule,
    RouterLink
  ],
  templateUrl: './order-summary.component.html',
  styleUrl: './order-summary.component.scss'
})
export class OrderSummaryComponent {
  removeCouponCode() {
    throw new Error('Method not implemented.');
  }

  applyCouponCode() {
    throw new Error('Method not implemented.');
  }
  cartService = inject(CartService);
}
