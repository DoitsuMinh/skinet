import { Component, computed, effect, inject, signal } from '@angular/core';
import { ShopService } from 'src/app/core/services/shop.service';
import { Product } from 'src/app/models/product';
import { ShopParams } from 'src/app/models/shopParams';
import { ProductItemComponent } from "./product-item/product-item.component";
import { FormsModule, NgForm, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FilteredProduct } from 'src/app/models/filteredProduct';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

@Component({
  selector: 'app-shop',
  standalone: true,
  imports:
    [ProductItemComponent,
      FormsModule,
      MatFormFieldModule,
      MatInputModule,
      MatIconModule,
      ReactiveFormsModule,
      MatAutocompleteModule],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss'
})
export class ShopComponent {
  products: Product[] = [];
  shopService = inject(ShopService);
  filteredProducts = signal<FilteredProduct[]>([]);

  private searchSubject = new Subject<string>();

  ngOnInit(): void {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.loadProducts();

    this.searchSubject.pipe(
      // Debounce the search input. This means that the search will only be performed
      // after the user has stopped typing for 500 milliseconds. This prevents
      // excessive calls to the service while the user is typing.
      debounceTime(500),

      // Only emit a new value if it's different from the previous value. This prevents
      // duplicate searches from being performed if the user types the same thing twice.
      distinctUntilChanged()
    ).subscribe(searchValue => {
      this.handleSearch(searchValue);
    });
  }

  loadProducts(params?: ShopParams): void {
    this.shopService.getProducts(params ?? new ShopParams()).subscribe({
      next: response => this.products = response.data,
      error: error => console.error(error)
    })
  }

  onSubmit(searchForm: NgForm): void {
    const shopParams = new ShopParams(
      { pageNumber: 1, seachValue: searchForm.value.seachValue }
    );

    this.loadProducts(shopParams);
  }

  onSearchChanges(seachValue: string) {
    this.searchSubject.next(seachValue);
  }

  /**
   * Handles the search functionality. Fetches filtered products from the shop service
   * based on the search value.
   * @param searchValue The value entered in the search input.
   */
  private handleSearch(seachValue: string): void {
    // If the search value is less than 2 characters, clear the filtered products.
    if (seachValue.length < 2) {
      this.filteredProducts.set([]);
      return;
    }

    // Call the shop service to get filtered products.
    this.shopService.getFilteredProduct(seachValue).subscribe({
      next: (response: FilteredProduct[]) => this.updateFilteredProducts(response, seachValue),
      error: (error) => console.error(error)
    });
  }

  private updateFilteredProducts(response: FilteredProduct[], seachValue: string): void {
    this.filteredProducts.update((products) => {
      // If the response is shorter than or the same length as the current products,
      // replace the current products with the response.
      if (response.length <= products.length) {
        response.map(x => { x.htmlContent = this.handleHtmlContent(x.name, seachValue) });
        return response;
      }

      // Filter the response to find only new products (those not already in products).
      const newProducts = response.filter(
        (newProduct) => !products.some((existingProduct) => existingProduct.id === newProduct.id)
      );
      newProducts.map(x => { x.htmlContent = this.handleHtmlContent(x.name, seachValue) });
      // Append the new products to the existing products and return the updated array.
      return [...products, ...newProducts];
    });
  }

  private handleHtmlContent(content: string, searchValue: string): string {
    if (!searchValue.trim()) return content;

    const regex = new RegExp(`(${searchValue})`, 'gi'); // Case-insensitive match
    return '<span>' + content.replace(regex, '<strong>$1</strong>') + '</span>';
  }
}
