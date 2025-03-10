import { Component, inject, Input } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent } from '@angular/material/card'
import { CurrencyPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { Product } from 'src/app/shared/models/product';
import { CartService } from 'src/app/core/services/cart.service';

@Component({
  selector: 'app-product-item',
  standalone: true,
  imports: [MatCard, MatCardContent, CurrencyPipe, MatCardActions, MatButton, MatIcon, RouterLink],
  templateUrl: './product-item.component.html',
  styleUrl: './product-item.component.scss'
})
export class ProductItemComponent {
  @Input() product?: Product;

  cartService = inject(CartService);

}
