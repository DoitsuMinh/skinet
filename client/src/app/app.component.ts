import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from "./layout/header/header.component";
import { ShopService } from './core/services/shop.service';
import { Product } from './models/product';
import { ShopParams } from './shared/models/shopParams';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeaderComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  title = 'skinet';
  products: Product[] = [];
  shopService = inject(ShopService)

  ngOnInit(): void {
    this.shopService.getProducts(new ShopParams()).subscribe(res => {
      this.products = res
    })
  }
}
