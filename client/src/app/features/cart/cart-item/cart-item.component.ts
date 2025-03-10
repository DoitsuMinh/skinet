import { CurrencyPipe } from '@angular/common';
import { Component, inject, input } from '@angular/core';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { CartService } from 'src/app/core/services/cart.service';
import { CartItem } from 'src/app/shared/models/cart';

@Component({
  selector: 'app-cart-item',
  standalone: true,
  imports: [
    RouterLink,
    MatButton,
    MatIcon,
    CurrencyPipe,
    MatIconButton
  ],
  templateUrl: './cart-item.component.html',
  styleUrl: './cart-item.component.scss'
})
export class CartItemComponent {
  item = input.required<CartItem>();
  cartService = inject(CartService);

  incrementQuantity() {
    this.cartService.addItemToCart(this.item());
  }

  decrementQuantity() {
    this.cartService.removeItemToCart(this.item().productId);
  }

  removeItemFromCart() {
    this.cartService.removeItemToCart(this.item().productId, this.item().quantity);
  }
}
