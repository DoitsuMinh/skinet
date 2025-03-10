import { CurrencyPipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
import { CartService } from 'src/app/core/services/cart.service';
import { ShopService } from 'src/app/core/services/shop.service';
import { Product } from 'src/app/shared/models/product';

@Component({
  selector: 'app-product-details',
  standalone: true,
  imports: [
    CurrencyPipe,
    MatButton,
    MatIcon,
    MatFormField,
    MatInput,
    MatLabel,
    MatDivider,
    FormsModule
  ],
  templateUrl: './product-details.component.html',
  styleUrl: './product-details.component.scss'
})
export class ProductDetailsComponent implements OnInit {
  private shopService = inject(ShopService);
  private activatedRoute = inject(ActivatedRoute);
  private cartService = inject(CartService);
  product?: Product;
  quantity: number = 0;
  quantityInCart: number = 0;

  searchControl = new FormControl('');

  ngOnInit(): void {
    console.log('Welcome 👏👏!');
    this.loadProduct();
  }

  get btnText() {
    return this.quantityInCart > 0 ? 'Update cart' : 'Add to cart';
  }

  loadProduct() {
    const routeParam = this.activatedRoute.snapshot.paramMap.get('id');
    if (!routeParam) return;
    const id = Number(routeParam);
    if (typeof id !== 'number') return;

    this.shopService.getProduct(id).subscribe(result => {
      this.product = result;
      this.updateQuantityToCart();
    })
  }

  updateQuantityToCart() {
    this.quantityInCart = this.cartService.cart()?.items
      .find(x => x.productId === this.product.id)?.quantity || 0;
    this.quantity = this.quantityInCart || 1;
  }

  updateCart() {
    if (!this.product) return;
    if (this.quantity > this.quantityInCart) {
      const itemsToAdd = this.quantity - this.quantityInCart;
      this.quantityInCart += itemsToAdd;
      this.cartService.addItemToCart(this.product, itemsToAdd);
    } else {
      const itemsToRemove = this.quantityInCart - this.quantity;
      this.quantityInCart -= itemsToRemove;
      this.cartService.removeItemToCart(this.product.id, itemsToRemove);
    }
  }
}
