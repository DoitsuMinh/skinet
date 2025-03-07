import { CurrencyPipe } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormControl, FormsModule } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { MatDivider } from '@angular/material/divider';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { ActivatedRoute } from '@angular/router';
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
  product?: Product;

  quantity: number = 1;
  quantityInCart: number = 0;

  searchControl = new FormControl('');

  ngOnInit(): void {
    console.log('Welcome 👏👏!');
    this.loadProduct();
  }

  get btnText() {
    return 'Add to cart'
  }

  loadProduct() {
    const routeParam = this.activatedRoute.snapshot.paramMap.get('id');
    if (!routeParam) return;
    const id = Number(routeParam);
    if (typeof id !== 'number') return;

    this.shopService.getProduct(id).subscribe(result => {
      this.product = result;
    })
  }

  updateCart() {

  }
}
